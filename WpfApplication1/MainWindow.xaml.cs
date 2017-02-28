using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Word = Microsoft.Office.Interop.Word;
using System.Reflection;

using Newtonsoft.Json;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        


        public MainWindow()
        {
            InitializeComponent();

            first.Visibility = Visibility.Visible;
            second.Visibility = Visibility.Collapsed;

        }

        private void preview_but_Click(object sender, RoutedEventArgs e)
        {
            int numOfQuestions;

            try
            {
                if (testName_txt.Text.Length == 0)
                    throw new Exception("Test name can't be empty!");
                else if (!int.TryParse(numOfQues_txt.Text, out numOfQuestions))
                    throw new Exception("Can't have 0 questoins!");



                Question_grid q = new Question_grid();

                q.numOfQuestionEntered = numOfQuestions;
                for (int i = 0; i < numOfQuestions; i++)
                {
                    
                    
                    string selected = ((ComboBoxItem)default_answer_num.SelectedItem).Content.ToString();
                    Console.WriteLine("selected"+selected);
                    questions_panel.Children.Add(q.makeQuestionGrid((i + 1)));
                    q.makeDefaultAnswers((i + 1), Convert.ToInt16(selected));
                    
                    ComboBox comb =(ComboBox)q.controls["question_cb_" + (i + 1)];
                    int selected_item = Convert.ToInt16(selected);
                    comb.SelectedItem = selected_item;
                    
                    
                }
                saveBut_grid.Children.Add(q.makeSave_but());
                
                
                first.Visibility = Visibility.Collapsed;
                second.Visibility = Visibility.Visible;
            }

            catch(Exception error)
            {

                //System.Windows.MessageBox.Show("Wrong Format","", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Windows.MessageBox.Show(error.Message, "Wrong Format", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }
    }


    public class Question_grid : MainWindow
    {
        public Dictionary<string, Object> controls = new Dictionary<string, object>();
        /////////////////////////
        int answersMax = 0;
        int defaultFontSize = 14;
        int margin = 40;
        char[] alpha = "ⒶⒷⒸⒹⒺⒻⒼⒽⒾⒿⓀⓁⓂⓃⓄⓅⓆⓇⓈⓉⓊⓋⓌⓍⓎⓏ".ToCharArray();
        char[] num = "⓪①②③④⑤⑥⑦⑧⑨".ToCharArray();
        string s = "";
        public static int idNumber = 8;
        public int startid = 3 + idNumber;
        public int endid = 3 + idNumber + idNumber;
        /////////////////////////



        //question

        public int numOfQuestionEntered;


        TextBlock grid_num;

        //number of answers




        public Button makeSave_but()
        {
            Button save_but = new Button();
            save_but.Height = 30;
            save_but.Name = "save_but";
            save_but.Content = "Create Files";

            save_but.Click += new RoutedEventHandler(save_Click);
            return save_but;


        }

        public Grid makeQuestionGrid(int number)
        {
            //intialize grid
            Grid question_grid = new Grid();
            question_grid.Name = "question_g_" + number;
            BrushConverter bc = new BrushConverter();
            question_grid.Background = (Brush)bc.ConvertFrom("#FFC2C6C9"); 
            question_grid.Margin = new Thickness(10, 10, 10, 0);

            System.Windows.Media.Effects.DropShadowEffect shadowEffect = new System.Windows.Media.Effects.DropShadowEffect();
            shadowEffect.BlurRadius = 14;
            shadowEffect.Opacity = 0.5;
            question_grid.Effect = shadowEffect;

            grid_num = new TextBlock();
            grid_num.Text = "Question " + (number) + " :";
            grid_num.Foreground = Brushes.Black;
            question_grid.Children.Add(grid_num);

            //set cloumn
            ColumnDefinition col = new ColumnDefinition();
            col.Width = new GridLength(4, GridUnitType.Star);
            question_grid.ColumnDefinitions.Add(col);
            col = new ColumnDefinition();
            col.Width = new GridLength();
            question_grid.ColumnDefinitions.Add(col);
            col = new ColumnDefinition();
            col.Width = new GridLength(5, GridUnitType.Star);
            question_grid.ColumnDefinitions.Add(col);

            // textbox
            TextBox question_txt = new TextBox();
            question_txt.Margin = new Thickness(10, 30, 10, 10);
            question_txt.Height = 70;
            question_txt.FontSize = 20;
            question_txt.TextWrapping = TextWrapping.Wrap;
            Grid.SetColumn(question_txt, 0);
            question_txt.Name = "question_txt_" + number;

            controls.Add(question_txt.Name, question_txt);

            question_grid.Children.Add(question_txt);


            //combobox

            ComboBox numOfAnmsers_cb = new ComboBox();
            numOfAnmsers_cb.Margin = new Thickness(10);
            numOfAnmsers_cb.Height = 30;
            Grid.SetColumn(numOfAnmsers_cb, 1);
            numOfAnmsers_cb.Name = "question_cb_" + number;
           
            for (int i = 2; i <= 5; i++)
            {
                numOfAnmsers_cb.Items.Add(i);
            }
            numOfAnmsers_cb.SelectionChanged += new SelectionChangedEventHandler(makeAnswersGrid);
            question_grid.Children.Add(numOfAnmsers_cb);
            controls.Add(numOfAnmsers_cb.Name, numOfAnmsers_cb);

            controls.Add(question_grid.Name, question_grid);
            
            return question_grid;

        }


        private void save_Click(object sender, RoutedEventArgs e)
        {
            fill_matrix(numOfQuestionEntered);
        }


        public void fill_matrix(int numOfQuestionsEntered)
        {

            Question[] questions = new Question[numOfQuestionEntered];

            for (int i = 0; i < numOfQuestionsEntered; i++)
            {
                questions[i] = new Question();

                //question 
                TextBox question_txt = (TextBox)controls["question_txt_" + (i + 1)];
                questions[i].text = question_txt.Text;

                //answers
                ComboBox numOfAnswers_cb = (ComboBox)controls["question_cb_" + (i + 1)];
                Console.WriteLine(questions[i].text);

                questions[i].answers = new string[Convert.ToInt16(numOfAnswers_cb.SelectedItem)];
                questions[i].correctAnswer = 0;
                for (int j = 0; j < questions[i].answers.Length; j++)
                {
                    TextBox answer_txt = (TextBox)controls["answer_txt_" + (i + 1) + "_" + (j + 1)];
                    questions[i].answers[j] = answer_txt.Text;
                    Console.WriteLine(answer_txt.Text);
                    RadioButton rb = (RadioButton)controls["answer_rb_" + (i + 1) + "_" + (j + 1)];


                    if (rb.IsChecked.Value)
                    {
                        questions[i].correctAnswer = j;
                    }
                }
                


                Console.WriteLine("question NUM : " + questions.Length);
                Console.WriteLine("question :" + questions[i].text);
                Console.WriteLine("NUM OF ANSWERS : " + questions[i].answers.Length);
                Console.WriteLine("correct answer" + questions[i].correctAnswer);
                Console.WriteLine("correct answer " + questions[i].answers[questions[i].correctAnswer]);
                Console.WriteLine("******************************");


            }//end of for

            createAnswerSheet(questions);
            createQuestionsSheet(questions);
            saveTest(questions);
        }


        public void makeDefaultAnswers(int number, int defaultNumber)
        {

            Grid grid = (Grid)controls["question_g_" + number];
            StackPanel answer_stack = new StackPanel();
            StackPanel answer_substack = new StackPanel();

            answer_substack.Name = "answer_stack_" + number;
            controls.Add(answer_substack.Name, answer_substack);
            string[] question_num = grid.Name.Split('_');//get question number



            for (int i = 0; i < defaultNumber; i++)
            {
                Grid g = new Grid();

                g.Name = "answer_g_" + (question_num[2]) + (i + 1);
                g.Margin = new Thickness(10, 0, 10, 10);
                BrushConverter bc = new BrushConverter();
                g.Background = (Brush)bc.ConvertFrom("#FFC2C6C9");
                
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(5, GridUnitType.Star);
                g.ColumnDefinitions.Add(col);
                col = new ColumnDefinition();
                col.Width = new GridLength(17, GridUnitType.Star);
                g.ColumnDefinitions.Add(col);


                RadioButton rb = new RadioButton();
                rb.Margin = new Thickness(10);
                rb.Name = "answer_rb_" + (question_num[2]) + "_" + (i + 1);
                rb.GroupName = "question_rbg_" + (question_num[2]);


                g.Children.Add(rb);

                Grid.SetColumn(rb, 0);

                TextBox txt = new TextBox();
                txt.Name = "answer_txt_" + (question_num[2]) + "_" + (i + 1);
                txt.Margin = new Thickness(10);

                // answers_txts.Add(txt);
                g.Children.Add(txt);
                Grid.SetColumn(txt, 1);

                controls[txt.Name] = txt;
                controls[rb.Name] = rb;
                controls[g.Name] = g;

                answer_substack.Margin = new Thickness(0, 10, 0, 0);
                answer_substack.Children.Add(g);
            }
            answer_stack.Background = Brushes.White;
            Grid.SetColumn(answer_stack, 2);
            answer_stack.Children.Add(answer_substack);


            grid.Children.Add(answer_stack);
        }






        public void makeAnswersGrid(object sender, EventArgs e)
        {


            ComboBox comboBox = (ComboBox)sender;
            Grid grid = (Grid)comboBox.Parent;

            StackPanel answer_substack = (StackPanel)controls["answer_stack_" + comboBox.Name.Split('_')[comboBox.Name.Split('_').Length - 1]];

            //get old answers
            string[] oldAnswers = new string[answer_substack.Children.Count];
            TextBox answer_txt;
            RadioButton answer_radio;
            int oldAnswer = 0;
            bool isAnswerSelected = false;
            for (int i = 0; i < answer_substack.Children.Count; i++)
            {
                answer_txt = (TextBox)controls["answer_txt_" + comboBox.Name.Split('_')[comboBox.Name.Split('_').Length - 1] + "_" + (i + 1)];
                answer_radio = (RadioButton)controls["answer_rb_" + comboBox.Name.Split('_')[comboBox.Name.Split('_').Length - 1] + "_" + (i + 1)];
                
                oldAnswers[i] = answer_txt.Text;
                if (answer_radio.IsChecked==true)
                    oldAnswer = i;
            }
            answer_substack.Children.Clear();

            int selectednumber = Convert.ToInt16(comboBox.SelectedItem);//get selected number


            string[] question_num = grid.Name.Split('_');//get question number



            for (int i = 0; i < selectednumber; i++)
            {
                Grid g = new Grid();

                g.Name = "answer_g_" + (question_num[2]) + (i + 1);
                g.Margin = new Thickness(10, 0, 10, 10);
                BrushConverter bc = new BrushConverter();
                g.Background = (Brush)bc.ConvertFrom("#FFC2C6C9");
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(5, GridUnitType.Star);
                g.ColumnDefinitions.Add(col);
                col = new ColumnDefinition();
                col.Width = new GridLength(17, GridUnitType.Star);
                g.ColumnDefinitions.Add(col);


                RadioButton rb = new RadioButton();
                rb.Margin = new Thickness(10);
                rb.Name = "answer_rb_" + (question_num[2]) + "_" + (i + 1);
                rb.GroupName = "question_rbg_" + (question_num[2]);
                rb.Content = alpha[i];
                rb.FontSize = 16;
                if (i == oldAnswer)
                {
                    rb.IsChecked = true;
                    isAnswerSelected = true;
                }


                g.Children.Add(rb);
                Grid.SetColumn(rb, 0);

                TextBox txt = new TextBox();
                txt.Name = "answer_txt_" + (question_num[2]) + "_" + (i + 1);
                txt.Margin = new Thickness(10);

                if(i<oldAnswers.Length)
                    txt.Text = oldAnswers[i];

                // answers_txts.Add(txt);
                g.Children.Add(txt);
                Grid.SetColumn(txt,1);

                Console.WriteLine(txt.Name);
                controls[txt.Name] = txt;
                controls[rb.Name] = rb;
                controls[g.Name] = g;

                answer_substack.Margin = new Thickness(0, 10, 0, 0);
                answer_substack.Children.Add(g);
            }
            if (!isAnswerSelected)
                ((RadioButton)controls["answer_rb_" + comboBox.Name.Split('_')[comboBox.Name.Split('_').Length - 1] + "_1"]).IsChecked = true;

        }


        private void saveTest(Question[] questions)
        {
            System.IO.File.WriteAllText(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+"\\"+testName_txt.Text+".json", JsonConvert.SerializeObject(questions));
            System.Windows.MessageBox.Show("Test saved to MyDocuments folder.", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void createAnswerSheet(Question[] questions)
        {

            for (int i = 0; i < questions.Length; i++)
                if (questions[i].answers.Length > answersMax)
                    answersMax = questions[i].answers.Length;

            object oMissing = System.Reflection.Missing.Value;
            object oEndOfDoc = "\\endofdoc";

            Word._Application oWord;
            Word._Document oDoc;
            oWord = new Word.Application();
            oWord.Visible = true;
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc.Paragraphs.ReadingOrder = Word.WdReadingOrder.wdReadingOrderLtr;
            oWord.ActiveDocument.PageSetup.TopMargin = margin;
            oWord.ActiveDocument.PageSetup.LeftMargin = margin;
            oWord.ActiveDocument.PageSetup.RightMargin = margin;
            oWord.ActiveDocument.PageSetup.BottomMargin = margin;
            Word.Table headerTable;
            Word.Range wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;

            headerTable = oDoc.Tables.Add(wrdRng, 1, 1);
            headerTable.Range.ParagraphFormat.SpaceAfter = 1;
            headerTable.Range.Font.Size = defaultFontSize;

            headerTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            headerTable.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;

            //headerTable.Range.Cells[1].Range.ImportFragment= "";
            headerTable.Cell(1, 1).Split(2, 1);
            headerTable.Range.Cells[1].Split(1, 2);
            headerTable.Range.Cells[1].Width = (headerTable.Range.Cells[1].Width / 50) * 65;
            headerTable.Range.Cells[2].Width = (headerTable.Range.Cells[2].Width / 50) * 35;

            headerTable.Range.Cells[2].Split(3, 1);
            headerTable.Range.Cells[2].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            headerTable.Range.Cells[2].Range.Text = "Student ID";


            headerTable.Range.Cells[3].Split(1, idNumber);
            headerTable.Range.Cells[startid].Split(1, idNumber);

            s = "";
            for (int column = 0; column < 10; column++)
            {
                s += (num[column].ToString());
                if (column < 9)
                    s += "\n";

            }

            for (; startid < endid; startid++)
            {

                headerTable.Range.Cells[startid].Range.ParagraphFormat.LineSpacing = 12;
                headerTable.Range.Cells[startid].Range.Font.Size = 15;
                headerTable.Range.Cells[startid].Range.Font.Bold = 1;
                headerTable.Range.Cells[startid].Range.Text = s;
            }
            headerTable.Range.Cells[(endid)].Split(1, 4);

            string answerLine = "\n\n";

            headerTable.Range.Cells[endid].Range.Text = String.Empty;
            for (int raw = 0, col = endid; raw < 100 && col < (endid + 4); raw++)
            {
                answerLine += ((raw + 1).ToString() + "-\t");

                for (int j = 0; j < 5; j++)
                {
                    answerLine += alpha[j] + " ";
                }

                //answerLine += "\n";
                headerTable.Range.Cells[col].Range.ParagraphFormat.LineSpacing = 9;
                headerTable.Range.Cells[col].Range.Font.Size = 15;
                headerTable.Range.Cells[col].Range.Font.Bold = 1;
                headerTable.Range.Cells[col].Range.Text = answerLine;

                if ((raw + 1) % 25 == 0)
                {
                    col += 1;
                    answerLine = "\n\n";

                }
                else
                    answerLine += "\n";
            }

        }

        private void createQuestionsSheet(Question[] questions)
        {
            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            object oMissing = System.Reflection.Missing.Value;
            object oEndOfDoc = "\\endofdoc";

            Word._Application oWord;
            Word._Document oDoc;
            oWord = new Word.Application();
            oWord.Visible = true;

            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc.Paragraphs.ReadingOrder = Word.WdReadingOrder.wdReadingOrderLtr;

            string answerLine = "";
            for (int raw = 0; raw < questions.Length; raw++)
            {

                oDoc.Content.InsertAfter((raw + 1) + "- " + questions[raw].text + "\n");

                answerLine = "\t";
                for (int column = 0; column < questions[raw].answers.Length; column++)
                    answerLine += alpha[column] + ". " + questions[raw].answers[column] + "\t";

                oDoc.Content.InsertAfter(answerLine + "\n");
            }

        }

    }




    public class Question
    {
        public string text;
        public string[] answers;
        public int correctAnswer;
    }
}
