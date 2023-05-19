using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsApp_Auto_Newslatter
{
    class ArrayHandler
    {
        private const string PathForFileSaving = "C:/Users/Информатик/Documents/Списки номеров (WhatsApp AutoSendler)/Готовые списки/Список.txt";
        private const char TabulationSymbol = '\t';

        public List<ContactCell> HandleFileWithNumbers(string filePath)
        {
            List<ContactCell> contactCells = new List<ContactCell>();
            ContactCell contactCellForAdding;
            var fileLinesArray = File.ReadAllLines(filePath);

            foreach (var line in fileLinesArray)
            {
                string newContactCellName = "";
                bool isNameEnded = false;
                string newContactCellPhoneNumber = "";

                foreach (var symbol in line)
                {
                    if (symbol == TabulationSymbol)
                        isNameEnded = true;

                    if (!isNameEnded)
                    {
                        if (symbol != TabulationSymbol)
                            newContactCellName += symbol;
                    }
                    else
                    {
                        if (symbol != TabulationSymbol)
                            newContactCellPhoneNumber += symbol;
                    }
                }

                contactCellForAdding = new ContactCell(newContactCellName, newContactCellPhoneNumber);

                var contactCellsPhoneNumbers = from ContactCell contactCell in contactCells
                                               select contactCell.PhoneNumber;

                if (!contactCellsPhoneNumbers.Contains(contactCellForAdding.PhoneNumber))
                    contactCells.Add(contactCellForAdding);
            }

            return contactCells;
        }

        public void CreateOrUpdateFileWithContactCells(List<ContactCell> contactCells)
        {
            List<string> linesForAppending = new List<string>();

            foreach (var contactCell in contactCells)
                linesForAppending.Add(contactCell.FullName + TabulationSymbol + contactCell.PhoneNumber);

            if (File.Exists(PathForFileSaving))
            {
                string[] fileLines = File.ReadAllLines(PathForFileSaving);

                foreach (var lineForAppending in linesForAppending)
                {
                    if (!fileLines.Contains(lineForAppending))
                        File.AppendAllText(PathForFileSaving, lineForAppending + '\n');
                }
            }
            else
            {
                File.Create(PathForFileSaving);
                File.AppendAllLines(PathForFileSaving, linesForAppending);
            }
        }
    }
}