using System;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace UndertaleSaveManager {
    public class SaveManager : Form {
        [STAThread]
        static public void Main() {
            Application.Run(new SaveManager());
        }

        public SaveManager() {
            this.Text = "Undertale Save Manager";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            Directory.CreateDirectory("./UndertaleSaves");

            Button backup = new Button() { Text = "Backup current files as...", Left = 50, Width = 200, Top = 50 };
            backup.Click += new EventHandler(Backup);
            Controls.Add(backup);

            Button load = new Button() { Text = "Load backed up files...", Left = 50, Width = 200, Top = 100 };
            load.Click += new EventHandler(LoadBackup);
            Controls.Add(load);

            Button clear = new Button { Text = "Delete current files to start anew...", Left = 50, Width = 200, Top = 150 };
            clear.Click += new EventHandler(ClearFiles);
            Controls.Add(clear);

        }

        private void Backup(object sender, EventArgs e) {
            string[] currentFiles = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/UNDERTALE");
            string backupName = Prompt.ShowDialog("What is this backup called?", "Backup");
            if(backupName != "") {
                Directory.CreateDirectory("./UndertaleSaves/" + backupName);
                foreach(string file in currentFiles) {
                    Console.WriteLine("Copying " + Path.GetFileName(file) + "...");
                    File.Copy(file, "./UndertaleSaves/" + backupName + "/" + Path.GetFileName(file), true);
                }

                File.Create("./UndertaleSaves/" + backupName + "/" + backupName + ".undertale");

                Console.WriteLine("Done.");
                MessageBox.Show("Backed up!");
            }
        }

        private void LoadBackup(object sender, EventArgs e) {
            Console.WriteLine("Loading backup...");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "undertale files (*.undertale)|*.undertale";
            openFileDialog.RestoreDirectory = true;
            Console.WriteLine("Dialog Created");

            if(openFileDialog.ShowDialog() == DialogResult.OK) {
                string[] backupFiles = Directory.GetFiles("./UndertaleSaves/" + Path.GetFileNameWithoutExtension(openFileDialog.FileName));

                foreach(string file in backupFiles) {
                    if(Path.GetExtension(file) != ".undertale") {
                        Console.WriteLine("Copying " + Path.GetFileName(file) + "...");
                        File.Copy(file, Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/UNDERTALE/" + Path.GetFileName(file), true);
                    }
                }

                Console.WriteLine("Done.");
                MessageBox.Show("Loaded!");
            }
        }

        private void ClearFiles(object sender, EventArgs e) {
            string[] currentFiles = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/UNDERTALE");

            foreach(string file in currentFiles) {
                Console.WriteLine("Deleting " + file + "...");
                File.Delete(file);
            }

            Console.WriteLine("Done");
            MessageBox.Show("Deleted!");

        }

    }

    public class Prompt {
        public static string ShowDialog(string text, string caption) {
            Form prompt = new Form();
            prompt.Width = 500;
            prompt.Height = 150;
            prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
            prompt.Text = caption;
            prompt.StartPosition = FormStartPosition.CenterScreen;
            Label textLabel = new Label() { Left = 50, Top = 20, Width = 400, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }

}