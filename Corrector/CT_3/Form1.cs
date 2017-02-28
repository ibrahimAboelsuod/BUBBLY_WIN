using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math;
using AForge.Math.Geometry;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Newtonsoft.Json;

namespace CT_3
{
    public partial class Form1 : Form
    {
        /*---files---*/
        private string rootPath;
        private string[] examImages;
        private string testJSON;

        List<Blob> circleBlobs;
        /*---images---*/
        UnmanagedImage orginalImage;
        UnmanagedImage grayImage;

        /*---filters properties---*/
        private int brightnessLevel = 25;
        private int smoothFactor = 200;
        private int thresholdLevel = 90;
        private int minBlobHeight = 20;
        private int minBlobWidth = 20;
        private double fullnessAvg = 0.6;
        private int yMinDis = 3;

        /*---filters---*/
        BrightnessCorrection brightnessFilter;
        Invert invertFilter;
        DocumentSkewChecker skewChecker = new DocumentSkewChecker();
        AdaptiveSmoothing smoothFilter;
        Threshold thresholdFilter;
        BlobsFiltering blobFilter;

        /*---tools---*/
        BlobCounter blobCounter;
        SimpleShapeChecker shapeChecker;

        /*---others---*/
        Pen bluePen = new Pen(Color.Blue, 7);     //is circle
        Pen yellowPen = new Pen(Color.Yellow, 7); //is double
        Pen redPen = new Pen(Color.Red, 7);       //is wrong
        Pen greenPen = new Pen(Color.Green, 7);   //is answer


        //
        public Form1()
        {
            InitializeComponent();

            /*---filters---*/
            brightnessFilter = new BrightnessCorrection(brightnessLevel);
            invertFilter = new Invert();
            smoothFilter = new AdaptiveSmoothing(smoothFactor);
            thresholdFilter = new Threshold(thresholdLevel);
            blobFilter = new BlobsFiltering();

            /*---tools---*/
            blobCounter = new BlobCounter();
            shapeChecker = new SimpleShapeChecker();
        }


        /*---Open folder---*/
        private void examFolderMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                rootPath = folderBrowser.SelectedPath;
                examImages = Directory.EnumerateFiles(folderBrowser.SelectedPath, "*.*", SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(".jpg") || s.EndsWith(".png")).ToArray();
                string[]jsonFiles = Directory.EnumerateFiles(rootPath, "*.*", SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(".json")).ToArray();

                if (examImages.Length == 0 || jsonFiles.Length==0)
                    MessageBox.Show("You are missing JSON or image files", "Error");
                else
                {
                    testJSON = jsonFiles[0];
                    prepareSequence();
                }
            }
        }
        
        /*---prepare workspace & inaite tools*/
        private void prepareSequence()
        {
            preparationBox.Enabled = true;
            correctBtn.Enabled = true;
            correctAllBtn.Enabled = true;

            pictureBox.Image = System.Drawing.Image.FromFile(examImages[0]);

            pictureBox.Image = previewCorrectionManaged(0);
            ResizeAndDisplayImage();
        }
        
        /*---correction preview---*/
        private UnmanagedImage previewCorrection(int imgID)
        {
            orginalImage = UnmanagedImage.FromManagedImage((Bitmap)System.Drawing.Image.FromFile(examImages[imgID]));
            grayImage = UnmanagedImage.Create(orginalImage.Width, orginalImage.Height, PixelFormat.Format8bppIndexed);
            //bright into gray
            Grayscale.CommonAlgorithms.BT709.Apply(brightnessFilter.Apply(orginalImage), grayImage);
            //invert
            invertFilter.ApplyInPlace(grayImage);

            // create rotation filter
            RotateBilinear rotationFilter = new RotateBilinear(-skewChecker.GetSkewAngle(grayImage));
            rotationFilter.FillColor = Color.Black;
            //rotationFilter.KeepSize = true;
            // rotate filter
            grayImage = rotationFilter.Apply(grayImage);

            //smooth
            smoothFilter.ApplyInPlace(grayImage);
            //threshold
            thresholdFilter.ApplyInPlace(grayImage);
            //size filter
            blobFilter.CoupledSizeFiltering = true;
            blobFilter.MinWidth = minBlobWidth;
            blobFilter.MinHeight = minBlobHeight;
            blobFilter.ApplyInPlace(grayImage);

            //shape filter
            blobCounter.ProcessImage(grayImage);
            Blob[] blobs = blobCounter.GetObjectsInformation();
            circleBlobs = new List<Blob>();

            int anchorLeft = 0;
            int anchorRight = 0;
            
            Bitmap detactFilter = new Bitmap(grayImage.ToManagedImage());
            Graphics graphics = Graphics.FromImage(detactFilter);
            List<IntPoint> edgePoints;

            AForge.Point center;
            float radius;

            for (int i = 0; i<blobs.Length; i++)
            {
                edgePoints = blobCounter.GetBlobsEdgePoints(blobs[i]);
                // is circle
                if (shapeChecker.IsCircle(edgePoints, out center, out radius))
                {
                    circleBlobs.Add(blobs[i]);
                    graphics.DrawEllipse(bluePen, (float)(center.X - radius), (float)(center.Y - radius), (float)(radius * 2), (float)(radius * 2));

                    //is answerd?
                    if (blobs[i].Fullness > fullnessAvg)
                    {
                        graphics.DrawEllipse(greenPen, (float)(center.X - radius), (float)(center.Y - radius), (float)(radius * 2), (float)(radius * 2));

                        //understand comulmn limits
                        for (int d = 4; d >= 1; d--)
                        {
                            if (blobs[i].Rectangle.X < grayImage.Width / 4 * d)
                            {
                                anchorRight = grayImage.Width / 4 * d;
                                anchorLeft = (grayImage.Width / 4 * d) - grayImage.Width / 4;
                            }
                        }

                        for (int j = i+1; j < blobs.Length; j++)
                        {

                            if (blobs[j].Fullness > fullnessAvg && Math.Abs(blobs[i].Rectangle.Y - blobs[j].Rectangle.Y) < yMinDis && blobs[j].Rectangle.X < anchorRight && blobs[j].Rectangle.X > anchorLeft)
                            {
                                graphics.DrawEllipse(yellowPen, (float)(center.X - radius), (float)(center.Y - radius), (float)(radius * 2), (float)(radius * 2));
                            }
                        }

                        



                    }//end of is answerd
                }//end of is cricle


            }//end of for


            return grayImage;
        }
        private System.Drawing.Image previewCorrectionManaged(int imgID)
        {
            orginalImage = UnmanagedImage.FromManagedImage((Bitmap)System.Drawing.Image.FromFile(examImages[imgID]));
            grayImage = UnmanagedImage.Create(orginalImage.Width, orginalImage.Height, PixelFormat.Format8bppIndexed);
            //bright into gray
            Grayscale.CommonAlgorithms.BT709.Apply(brightnessFilter.Apply(orginalImage), grayImage);
            //invert
            invertFilter.ApplyInPlace(grayImage);

            // create rotation filter
            RotateBilinear rotationFilter = new RotateBilinear(-skewChecker.GetSkewAngle(grayImage));
            rotationFilter.FillColor = Color.Black;
            //rotationFilter.KeepSize = true;
            // rotate filter
            grayImage = rotationFilter.Apply(grayImage);

            //smooth
            smoothFilter.ApplyInPlace(grayImage);
            //threshold
            thresholdFilter.ApplyInPlace(grayImage);
            //size filter
            blobFilter.CoupledSizeFiltering = true;
            blobFilter.MinWidth = minBlobWidth;
            blobFilter.MinHeight = minBlobHeight;
            blobFilter.ApplyInPlace(grayImage);

            //shape filter
            blobCounter.ProcessImage(grayImage);
            Blob[] blobs = blobCounter.GetObjectsInformation();
            circleBlobs = new List<Blob>();

            int anchorLeft = 0;
            int anchorRight = 0;

            Bitmap detactFilter = new Bitmap(grayImage.ToManagedImage());
            Graphics graphics = Graphics.FromImage(detactFilter);
            List<IntPoint> edgePoints;

            AForge.Point center;
            float radius;

            for (int i = 0; i < blobs.Length; i++)
            {
                edgePoints = blobCounter.GetBlobsEdgePoints(blobs[i]);
                // is circle
                if (shapeChecker.IsCircle(edgePoints, out center, out radius))
                {
                    circleBlobs.Add(blobs[i]);
                    graphics.DrawEllipse(bluePen, (float)(center.X - radius), (float)(center.Y - radius), (float)(radius * 2), (float)(radius * 2));

                    //is answerd?
                    if (blobs[i].Fullness > fullnessAvg)
                    {
                        graphics.DrawEllipse(greenPen, (float)(center.X - radius), (float)(center.Y - radius), (float)(radius * 2), (float)(radius * 2));

                        //understand comulmn limits
                        for (int d = 4; d >= 1; d--)
                        {
                            if (blobs[i].Rectangle.X < grayImage.Width / 4 * d)
                            {
                                anchorRight = grayImage.Width / 4 * d;
                                anchorLeft = (grayImage.Width / 4 * d) - grayImage.Width / 4;
                            }
                        }

                        for (int j = i + 1; j < blobs.Length; j++)
                        {

                            if (blobs[j].Fullness > fullnessAvg && Math.Abs(blobs[i].Rectangle.Y - blobs[j].Rectangle.Y) < yMinDis && blobs[j].Rectangle.X < anchorRight && blobs[j].Rectangle.X > anchorLeft)
                            {
                                graphics.DrawEllipse(yellowPen, (float)(center.X - radius), (float)(center.Y - radius), (float)(radius * 2), (float)(radius * 2));
                            }
                        }





                    }//end of is answerd
                }//end of is cricle


            }//end of for






            return (System.Drawing.Image)detactFilter;
        }
        private void preparationAuto_CheckedChanged(object sender, EventArgs e)
        {


            if (preparationAuto.Checked)
            {
                brightnessFilter.AdjustValue = brightnessLevel;
                smoothFilter.Factor = smoothFactor;
                thresholdFilter.ThresholdValue = thresholdLevel;
                fullnessAvg = 0.6;

                pictureBox.Image = previewCorrectionManaged(0);
                ResizeAndDisplayImage();
            }

            brightnessBar.Value = brightnessFilter.AdjustValue;
            label1.Text = "Brightness: " + brightnessBar.Value;

            smoothBar.Value = (int)smoothFilter.Factor;
            label2.Text = "Smooth: " + smoothBar.Value;

            thresholdBar.Value = thresholdFilter.ThresholdValue;
            label3.Text = "Threshold: " + thresholdBar.Value;

            answerRatioBar.Value = (int)(fullnessAvg * 100);
            label4.Text = "Answer Ratio: " + answerRatioBar.Value + "%";

            brightnessBar.Enabled = !brightnessBar.Enabled;
            smoothBar.Enabled = !smoothBar.Enabled;
            thresholdBar.Enabled = !thresholdBar.Enabled;
            answerRatioBar.Enabled = !answerRatioBar.Enabled;
            submitFilters.Enabled = !submitFilters.Enabled;

        }


        private System.Drawing.Point[] ToPointsArray(List<IntPoint> points)
        {
            System.Drawing.Point[] array = new System.Drawing.Point[points.Count];

            for (int i = 0, n = points.Count; i < n; i++)
            {
                array[i] = new System.Drawing.Point(points[i].X, points[i].Y);
            }

            return array;
        }

        

        private void brightnessBar_Scroll(object sender, EventArgs e)
        {
            label1.Text = "Brightness: " + brightnessBar.Value;
        }
        private void smoothBar_Scroll(object sender, EventArgs e)
        {
            label2.Text = "Smooth: " + smoothBar.Value;

        }
        private void thresholdBar_Scroll(object sender, EventArgs e)
        {
            label3.Text = "Threshold: " + thresholdBar.Value;
        }
        private void answerRatioBar_Scroll(object sender, EventArgs e)
        {
            label4.Text = "Answer Ratio: " + answerRatioBar.Value + "%";
        }

        private void submitFilters_Click(object sender, EventArgs e)
        {
            brightnessFilter.AdjustValue = brightnessBar.Value;
            smoothFilter.Factor = smoothBar.Value;
            thresholdFilter.ThresholdValue = thresholdBar.Value;
            fullnessAvg = ((double)answerRatioBar.Value / 100);

            pictureBox.Image = previewCorrectionManaged(0);
            ResizeAndDisplayImage();
        }

        private void correctBtn_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Sample:");
            Exam tExam = correct(grayImage, -1);
            ResizeAndDisplayImage();
        }

        private void correctAllBtn_Click(object sender, EventArgs e)
        {
            if (correctAllBtn.Text != "Cancel"){
                Console.WriteLine("--------------------------------------");
                backgroundWorker.RunWorkerAsync();
                correctAllBtn.Text = "Cancel";
            }
            else
                backgroundWorker.CancelAsync();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Exam[] exams = new Exam[examImages.Length];
            for (int i = 0; i < examImages.Length; i++)
            {
                exams[i] = correct(previewCorrection(i), i);
            }
        }



        private Exam correct(UnmanagedImage grayImage, int id)
        {
            Question[] questions = JsonConvert.DeserializeObject<List<Question>>(System.IO.File.ReadAllText(testJSON)).ToArray();
            Blob[] circles = circleBlobs.ToArray();
            Blob[][] circlesM = new Blob[100][];

            List<Blob> oneCircles = new List<Blob>();
            List<Blob> twoCircles = new List<Blob>();
            List<Blob> threeCircles = new List<Blob>();
            List<Blob> fourCircles = new List<Blob>();

            Bitmap detactFilter = new Bitmap(grayImage.ToManagedImage());
            Graphics graphics = Graphics.FromImage(detactFilter);
            List<IntPoint> edgePoints;
            int safeMarg = 15;
            for (int i = 0; i < circles.Length; i++)
            {
                if (circles[i].Rectangle.X >= 0 && circles[i].Rectangle.X <= (grayImage.Width / 4))
                    oneCircles.Add(circles[i]);
                else if (circles[i].Rectangle.X >= grayImage.Width / 4 && circles[i].Rectangle.X <= grayImage.Width / 2)
                    twoCircles.Add(circles[i]);
                else if (circles[i].Rectangle.X >= grayImage.Width / 2 && circles[i].Rectangle.X <= grayImage.Width - (grayImage.Width / 4) - safeMarg)
                    threeCircles.Add(circles[i]);
                else if (circles[i].Rectangle.X >= grayImage.Width - (grayImage.Width / 4) - safeMarg && circles[i].Rectangle.X <= grayImage.Width)
                    fourCircles.Add(circles[i]);
            }
            try
            {
                for (int i = 0; i < 25 * 5; i += 5)
                {
                    //one
                    circlesM[i / 5] = new Blob[5];
                    for (int j = 0; j < 5; j++)
                        circlesM[i / 5][j] = oneCircles[j + i];
                    Array.Sort(circlesM[i / 5], delegate(Blob x, Blob y) { return x.Rectangle.X.CompareTo(y.Rectangle.X); });
                    //two
                    circlesM[(i + 25 * 5) / 5] = new Blob[5];
                    for (int j = 0; j < 5; j++)
                        circlesM[(i + 25 * 5) / 5][j] = twoCircles[j + i];
                    Array.Sort(circlesM[(i + 25 * 5) / 5], delegate(Blob x, Blob y) { return x.Rectangle.X.CompareTo(y.Rectangle.X); });
                    //three
                    circlesM[(i + 25 * 10) / 5] = new Blob[5];
                    for (int j = 0; j < 5; j++)
                        circlesM[(i + 25 * 10) / 5][j] = threeCircles[j + i];
                    Array.Sort(circlesM[(i + 25 * 10) / 5], delegate(Blob x, Blob y) { return x.Rectangle.X.CompareTo(y.Rectangle.X); });
                    //four
                    circlesM[(i + 25 * 15) / 5] = new Blob[5];
                    for (int j = 0; j < 5; j++)
                        circlesM[(i + 25 * 15) / 5][j] = fourCircles[j + i];
                    Array.Sort(circlesM[(i + 25 * 15) / 5], delegate(Blob x, Blob y) { return x.Rectangle.X.CompareTo(y.Rectangle.X); });
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Image: "+id+" is not alligned correctly, or is not fully visible", "Error");
            }
            

            int answerCount;
            int answerIndex;
            int score = 0;
            int warnnings = 0;
            for (int qCount = 0; qCount < questions.Length; qCount++)
            {
                answerCount = 0;
                answerIndex = 0;
                for (int aCount = 0; aCount < 5; aCount++)
                {
                    if (circlesM[qCount][aCount].Fullness > fullnessAvg)
                    {
                        answerCount += 1;
                        answerIndex = aCount;
                    }
                }

                if (answerCount == 1)
                {
                    edgePoints = blobCounter.GetBlobsEdgePoints(circlesM[qCount][answerIndex]);
                    if (questions[qCount].correctAnswer == answerIndex)
                    {
                        graphics.DrawEllipse(greenPen,
                            (float)(circlesM[qCount][answerIndex].CenterOfGravity.X - circlesM[qCount][answerIndex].Rectangle.Width + 5),
                            (float)(circlesM[qCount][answerIndex].CenterOfGravity.Y - circlesM[qCount][answerIndex].Rectangle.Width + 5),
                            (float)((circlesM[qCount][answerIndex].Rectangle.Width - 5) * 2),
                            (float)((circlesM[qCount][answerIndex].Rectangle.Width - 5) * 2));
                        score += 1;
                    }
                    else
                        graphics.DrawEllipse(redPen,
                            (float)(circlesM[qCount][answerIndex].CenterOfGravity.X - circlesM[qCount][answerIndex].Rectangle.Width + 5),
                            (float)(circlesM[qCount][answerIndex].CenterOfGravity.Y - circlesM[qCount][answerIndex].Rectangle.Width + 5),
                            (float)((circlesM[qCount][answerIndex].Rectangle.Width - 5) * 2),
                            (float)((circlesM[qCount][answerIndex].Rectangle.Width - 5) * 2));
                }
                else
                {
                    edgePoints = blobCounter.GetBlobsEdgePoints(circlesM[qCount][0]);
                    graphics.DrawEllipse(yellowPen,
                        (float)(circlesM[qCount][0].CenterOfGravity.X - circlesM[qCount][0].Rectangle.Width + 5),
                        (float)(circlesM[qCount][0].CenterOfGravity.Y - circlesM[qCount][0].Rectangle.Width + 5),
                        (float)((circlesM[qCount][0].Rectangle.Width - 5) * 2),
                        (float)((circlesM[qCount][0].Rectangle.Width - 5) * 2));
                    warnnings += 1;
                }
            }//end of questions loop
            Console.WriteLine("ID: " + id + " score: " + score + " warnnings: " + warnnings);



            pictureBox.Image = (System.Drawing.Image)detactFilter;

            Exam exam = new Exam();
            exam.score = score;
            exam.id = id;

            return exam;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            correctAllBtn.Text = "Correcct All";
            ResizeAndDisplayImage();
        }






        /*-------------------------------------*/
        private void ResizeAndDisplayImage()
        {

            System.Drawing.Image _OriginalImage = pictureBox.Image;

            if (_OriginalImage == null)
                return;


            int sourceWidth = _OriginalImage.Width;
            int sourceHeight = _OriginalImage.Height;
            int targetWidth;
            int targetHeight;
            double ratio;


            if (sourceWidth > sourceHeight)
            {
                targetWidth = pictureBox.Width;
                ratio = (double)targetWidth / sourceWidth;
                targetHeight = (int)(ratio * sourceHeight);
            }
            else if (sourceWidth < sourceHeight)
            {
                targetHeight = pictureBox.Height;
                ratio = (double)targetHeight / sourceHeight;
                targetWidth = (int)(ratio * sourceWidth);
            }
            else
            {
                targetHeight = pictureBox.Height;
                targetWidth = pictureBox.Width;
            }

            int targetTop = (pictureBox.Height - targetHeight) / 2;
            int targetLeft = (pictureBox.Width - targetWidth) / 2;

            Bitmap tempBitmap = new Bitmap(pictureBox.Width, pictureBox.Height,
                                           PixelFormat.Format24bppRgb);

            tempBitmap.SetResolution(_OriginalImage.HorizontalResolution,
                                     _OriginalImage.VerticalResolution);

            // Create a Graphics object to further edit the temporary bitmap

            Graphics bmGraphics = Graphics.FromImage(tempBitmap);

            bmGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            bmGraphics.DrawImage(_OriginalImage,
                                 new Rectangle(targetLeft, targetTop, targetWidth, targetHeight),
                                 new Rectangle(0, 0, sourceWidth, sourceHeight),
                                 GraphicsUnit.Pixel);

            // Dispose of the bmGraphics object

            bmGraphics.Dispose();

            // Set the image of the picImage picturebox to the temporary bitmap

            pictureBox.Image = tempBitmap;
            //ResizeAndDisplayImage();
        }

        private void UpdateZoomedImage(object sender, MouseEventArgs e)
        {
            if (pictureBox.Image != null)
            {


                int zoomWidth = pictureZoomBox.Width / 2;
                int zoomHeight = pictureZoomBox.Height / 2;

                int halfWidth = zoomWidth / 2;
                int halfHeight = zoomHeight / 2;

                Bitmap tempBitmap = new Bitmap(zoomWidth, zoomHeight, PixelFormat.Format24bppRgb);
                Graphics bmGraphics = Graphics.FromImage(tempBitmap);

                bmGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                bmGraphics.DrawImage(pictureBox.Image,
                                     new Rectangle(0, 0, zoomWidth, zoomHeight),
                                     new Rectangle(e.X - halfWidth, e.Y - halfHeight,
                                     zoomWidth, zoomHeight), GraphicsUnit.Pixel);

                pictureZoomBox.Image = tempBitmap;

                bmGraphics.DrawLine(Pens.Black, halfWidth + 1,
                                    halfHeight - 4, halfWidth + 1, halfHeight - 1);
                bmGraphics.DrawLine(Pens.Black, halfWidth + 1, halfHeight + 6,
                                    halfWidth + 1, halfHeight + 3);
                bmGraphics.DrawLine(Pens.Black, halfWidth - 4, halfHeight + 1,
                                    halfWidth - 1, halfHeight + 1);
                bmGraphics.DrawLine(Pens.Black, halfWidth + 6, halfHeight + 1,
                                    halfWidth + 3, halfHeight + 1);

                bmGraphics.Dispose();

                pictureZoomBox.Refresh();
            }
        }



        
        

    }

    public class Question
    {
        public string text;
        public string[] answers;
        public int correctAnswer;
    }

    public class Exam
    {
        public int id;
        public int score;
    }
}
