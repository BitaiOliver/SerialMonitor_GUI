using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace SerialMonitor
{
    public partial class Form1 : Form
    {
        private int seq = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            cmbPorts.Items.AddRange(ports);
            //cmbPorts.SelectedIndex = 1;

            cmbBaudrate.Items.Add("9600");

            btnClose.Enabled = false;
            btnSend.Enabled = false;
            //btnReceive.Enabled = false;

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            btnOpen.Enabled = false;
            btnClose.Enabled = true;
            btnSend.Enabled = true;
            btnReceive.Enabled = true;

            try
            {
                serialPort1.PortName = cmbPorts.Text;
                serialPort1.BaudRate = Convert.ToInt32(cmbBaudrate.Text);
                serialPort1.Open();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            btnOpen.Enabled = true;
            btnClose.Enabled = false;
            btnSend.Enabled = false;
            btnReceive.Enabled = false;
            try
            {
                serialPort1.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if(serialPort1.IsOpen)
                {
                    serialPort1.WriteLine(txtSend.Text + Environment.NewLine);
                    txtSend.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnReceive_Click(object sender, EventArgs e)
        {
            try
            {
                string data = GetFormattedLogEntry();

                txtReceive.Text +=  data;

                if (serialPort1.IsOpen)
                {
                    // txtReceive.Text = serialPort1.ReadExisting();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            
            //txtReceive.Text = serialPort1.ReadExisting();
            SetText(serialPort1.ReadExisting());
        }

        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtReceive.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtReceive.Text += text;
            }
        }

        /* Returns the current time in a string wich is exactly 16 char long*/
        private string GetFormattedDate()
        {
            string date = " ";
            string padding = "";
            int temp = 0;
            char[] cut = { 'A', 'M', 'P' };

            DateTime dt = DateTime.Now;
            date += dt.ToLongTimeString();
            date = date.Trim(cut);
            date += "." + dt.Millisecond.ToString();

            temp = date.Length;
            for(int i = 0; i < (16 - temp); i++)
            {
                padding += "  ";
            }
            return date + padding;
        }
        /* Returns the formatted period in string. Length = 14 */
        private string GetFormattedPeriod(int period)
        {
            int digits = 1;
            int temp = period;
            string ret = "    ";
            if(1000000 > period)
            {
                while (0 < (temp / 10))
                {
                    digits++;
                    temp = temp / 10;
                }
                for (int i = 0; i < (6 - digits); i++)
                {
                    ret += " ";
                }
                ret += period.ToString();
                ret += "    ";
            }
            else
            {
                ret = "    Error!    ";
            }

            return ret;
        }
        /* Returns the formatted ID in string. Length = 14 */
        private string GetFormattedID(int id)
        {
            int digits = 1;
            int temp = id;
            string ret = "     ";

            if (1000 > id)
            {
                while (0 < (temp / 10))
                {
                    digits++;
                    temp = temp / 10;
                }
                for (int i = 0; i < (3 - digits); i++)
                {
                    ret += " ";
                }
                ret += id.ToString();
                ret += "      ";
            }
            else
            {
                ret = "    Error!    ";
            }
            return ret;
        }

        /* Returns the formatted scope in string. Length = 18 */
        private string GetFormattedScope(int scope)
        {
            string ret = "    ";

            switch(scope)
            {
                case 1:
                    {
                        ret = "    Data    ";
                        break;
                    }
                case 2:
                    {
                        ret = "  Response  ";
                        break;
                    }
                default:
                    {
                        ret = "   Error!   ";
                        break;
                    }
            }
            ret += "  ";
            return ret;
        }
        /* Returns the formatted data in string. Length = variable */
        private string GetFormattedData(int data)
        {
            string ret = "      ";
            ret += data.ToString();
            return ret;
        }
        /* Returns the formatted data in string. Length = variable */
        private string GetFormattedData(string data)
        {
            string ret = "      ";
            ret += data;
            return ret;
        }
        /* Returns the formatted data line. Length = variable */
        private string GetFormattedLogEntry()
        {
            string ret = "";

            switch (seq)
            {
                case 0:
                    {
                        ret = GetFormattedDate();
                        ret += GetFormattedPeriod(1234);
                        ret += GetFormattedID(555);
                        ret += GetFormattedScope(1);
                        ret += GetFormattedData(123456789);
                        seq++;
                        break;
                    }
                case 1:
                    {
                        ret = GetFormattedDate();
                        ret += GetFormattedPeriod(134);
                        ret += GetFormattedID(555);
                        ret += GetFormattedScope(1);
                        ret += GetFormattedData(123456789);
                        seq++;
                        break;
                    }
                case 2:
                    {
                        ret = GetFormattedDate();
                        ret += GetFormattedPeriod(1234);
                        ret += GetFormattedID(55);
                        ret += GetFormattedScope(1);
                        ret += GetFormattedData(123456789);
                        seq++;
                        break;
                    }
                case 3:
                    {
                        ret = GetFormattedDate();
                        ret += GetFormattedPeriod(1234);
                        ret += GetFormattedID(555);
                        ret += GetFormattedScope(2);
                        ret += GetFormattedData(123456789);
                        seq = 0;
                        break;
                    }

            }
            /*
            ret = GetFormattedDate();
            ret += GetFormattedPeriod(1234);
            ret += GetFormattedID(555);
            ret += GetFormattedScope(1);
            ret += GetFormattedData(123456789);*/
            ret += "\r\n";
            return ret;
        }




    }
}
