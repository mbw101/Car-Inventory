using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace CarInventory
{
    public partial class Form1 : Form
    {
        List<Car> inventory = new List<Car>();

        public Form1()
        {
            InitializeComponent();

            loadDB();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            string year, make, colour, mileage;

            year = yearInput.Text;
            make = makeInput.Text;
            colour = colourInput.Text;
            mileage = mileageInput.Text;

            Car c = new Car(year, make, colour, mileage);
            inventory.Add(c);

            outputLabel.Text = yearInput.Text = makeInput.Text = colourInput.Text = mileageInput.Text = "";
            yearInput.Focus();
            displayItems();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].make == makeInput.Text)
                {
                    inventory.RemoveAt(i);
                }
            }

            int index = inventory.FindIndex(car => car.make == makeInput.Text);

            if (index > -1)
            {
                inventory.RemoveAt(index);
            }

            displayItems();
        }

        public void displayItems()
        {
            outputLabel.Text = "";

            foreach (Car c in inventory)
            {
                outputLabel.Text += c.year + " "
                     + c.make + " "
                     + c.colour + " "
                     + c.mileage + "\n";
            }
        }

        public void loadDB()
        {
            string year, make, colour, mileage;

            XmlReader reader = XmlReader.Create("Resources/CarInventory.xml", null);

            // while there is another chunk to read
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Text)
                {
                    year = reader.ReadString();

                    reader.ReadToNextSibling("make");
                    make = reader.ReadString();

                    reader.ReadToNextSibling("colour");
                    colour = reader.ReadString();

                    reader.ReadToNextSibling("mileage");
                    mileage = reader.ReadString();

                    Car c = new Car(year, make, colour, mileage);
                    inventory.Add(c);
                }
            }

            reader.Close();
        }

        public void saveDB()
        {
            XmlWriter writer = XmlWriter.Create("Resources/CarInventory.xml", null);

            writer.WriteStartElement("Inventory");

            foreach (Car c in inventory)
            {
                writer.WriteStartElement("Car");

                writer.WriteElementString("year", c.year);
                writer.WriteElementString("make", c.make);
                writer.WriteElementString("colour", c.colour);
                writer.WriteElementString("mileage", c.mileage);

                writer.WriteEndElement();
            }

            writer.WriteEndElement();

            writer.Close();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            saveDB();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
