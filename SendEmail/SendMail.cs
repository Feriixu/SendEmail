using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//Diese 2 Zeilen bindet die .NET funktionen ein um Internetprotokolle zu benutzen
using System.Net;
using System.Net.Mail;

namespace SendEmail
{
    public partial class SendMail : Form
    {
        //Deklaration von nötigen Objekten
        NetworkCredential login;
        SmtpClient client;
        MailMessage msg;

        public SendMail()
        {
            InitializeComponent();
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            //Login Objekt mit den Werten Initialisieren
            login = new NetworkCredential(txtUsername.Text, txtPassword.Text);
            client = new SmtpClient(txtSMTP.Text);
            client.Port = Convert.ToInt32(txtPort.Text);
            client.EnableSsl = chkSSL.Checked;
            client.Credentials = login;
            msg = new MailMessage { From = new MailAddress(txtUsername.Text + txtSMTP.Text.Replace("smtp.", "@"), "Felix", Encoding.UTF8) };
            msg.To.Add(new MailAddress(txtTo.Text));
            /*
            if (!string.IsNullOrEmpty(txtCC.Text));
                msg.To.Add(new MailAddress(txtCC.Text));
            */
            msg.Subject = txtSubject.Text;
            msg.Body = txtMessage.Text;
            msg.BodyEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.Priority = MailPriority.Normal;
            msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
            string userstate = "Sending...";
            client.SendAsync(msg, userstate);
        }

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
                MessageBox.Show(string.Format("{0} send canceled.", e.UserState), "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (e.Error != null)
                MessageBox.Show(string.Format("{0} {1}", e.UserState, e.Error), "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Your Message has been successfully sent.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
