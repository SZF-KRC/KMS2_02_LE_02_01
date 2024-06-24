
using KMS2_02_LE_02_01.Model;

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace KMS2_02_LE_02_01.UploadData
{
    public class UploadCSV
    {

        private static List<Person> _person;

        /// <summary>
        /// Öffnet einen Dateiöffnungsdialog und lädt die Bücher aus der ausgewählten CSV-Datei.
        /// </summary>
        public static List<Person> Upload()
        {
            try
            {
                string filePath = OpenFile("Enter Persons data please...");
                if (filePath == null) { return null; }
                _person = new List<Person>();
                using (StreamReader sr = new StreamReader(filePath))
                {
                    sr.ReadLine();
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length > 0)
                        {                        
                            _person.Add(new Person { Name = parts[0], Surname = parts[1], Age = Int32.Parse(parts[2]), City = parts[3], Gender = parts[4] });
                        }
                    }
                }
            }
            catch (FileFormatException ex) { MessageBox.Show(ex.Message); }
            catch (FileNotFoundException ex) { MessageBox.Show(ex.Message); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            return _person;
        }

        /// <summary>
        /// Öffnet einen Dateiöffnungsdialog und gibt den Pfad der ausgewählten Datei zurück.
        /// </summary>
        /// <param name="prompt">Der Titel des Dateiöffnungsdialogs.</param>
        /// <returns>Der Pfad der ausgewählten Datei oder null, wenn keine Datei ausgewählt wurde.</returns>
        private static string OpenFile(string prompt)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Text files (*.csv)|*.csv",
                    Title = prompt
                };
                return openFileDialog.ShowDialog() == DialogResult.OK ? openFileDialog.FileName : null;
            }
            catch (FileNotFoundException ex) { MessageBox.Show(ex.Message); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            return null;
        }
    }
}
