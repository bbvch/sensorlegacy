//-------------------------------------------------------------------------------
// <copyright file="Program.cs" company="bbv Software Services AG">
//   Copyright (c) 2013
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

namespace SensorLegacy
{
    using System;
    using System.Net.Mail;

    using SensorLegacy.Events;
    using SensorLegacy.Reports;
    using SensorLegacy.Vhpt;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Awesome refactoring happens here when time is available!
                var rp = new TxtRprt();

                Console.WriteLine("Sirius Cybernetics Legacy Sensor Management v1.0");
                Console.WriteLine("=========================================================");
                Console.WriteLine("=========================================================");
                Console.WriteLine("Press any key to start sensors");
                Console.WriteLine("---------------------------------------------------------");
                Console.ReadLine();
                BlackHoleSensor b = new BlackHoleSensor();
                DoorSensor d = new DoorSensor(b);
                d.Initialize();
                d.StartObservation();

                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine("Press any key to stop sensors");
                Console.ReadLine();
                d.Close();
                b.Stop();

                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine("Press any key to exit the sensor management.");
                Console.ReadLine();
            }
            catch (StackOverflowException e)
            {
                MailMessage objeto_mail = new MailMessage();
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.Host = "smtp.internal.mycompany.com";
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("user", "Password");
                objeto_mail.From = new MailAddress("applicationdied@server.com");
                objeto_mail.To.Add(new MailAddress("support@server.com"));
                objeto_mail.Subject = "Discovered something undiscoverable";

                objeto_mail.Body = "Message" + e.Data + e.HResult + e.HelpLink
                                   + ((e.InnerException != null)
                                       ? e.InnerException + e.InnerException.HelpLink + e.InnerException.StackTrace
                                       : string.Empty) + e.Message + e.Source + e.StackTrace + e.TargetSite;

                client.Send(objeto_mail);
            }
        }
    }
}
