//-------------------------------------------------------------------------------
// <copyright file="TxtRprt.cs" company="bbv Software Services AG">
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

namespace SensorLegacy.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;
    using System.Text;

    using SensorLegacy.Contexts;

    /// <summary>
    /// This is so awesome! We might need this in the future!!!!
    /// </summary>
    public class TxtRprt : Reporter
    {
        private readonly StrRep _in;

        // CTOR!!!
        public TxtRprt()
        {
            this._in = new StrRep();
        }

        /// <inheritdoc />
        public void Report(RContext context)
        {
            this._in.Report(context);

            try
            {
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "CustomizationReport.txt"), this._in.ToString());
            }
            catch (Exception e)
            {
                // CatchetyCatch! Yackaaaa!
                Debug.Assert(e != null, "Just to make sure! Will never happen!"); // <-- DEBUGGING!!!!

                // Just in case we made it to here, YOU NEVER KNOW! I did actually see this in production
                // during full moon strange glitches occur I swear!
                // Copy-Paste from http://stackoverflow.com/questions/9201239/send-e-mail-via-smtp-using-c-sharp because it has 6 answers!!!!

                MailMessage mail = new MailMessage("you@yourcompany.com", "user@hotmail.com");
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = "smtp.google.com";
                mail.Subject = "this is a test email." + e.Message.Substring(0, 50); // 50 chars is safe for subject!!
                mail.Body = "this is my test email body" + e.StackTrace;
                client.Send(mail);
            }
        }

        private class StrRep : Reporter
        {
            private RContext _ctx;

            public void Report(RContext ctx)
            {
                this._ctx = ctx;
            }

            public override string ToString()
            {
                return Dump(this._ctx);
            }

            private static string Dump(RContext context)
            {
                var builder = new StringBuilder();

                context.Extensions.ToList().ForEach(e => Dump(e.Name, e.Description, builder, 0));

                Dump(context.Run, builder);
                Dump(context.Shutdown, builder);

                return builder.ToString();
            }

            private static void Dump(EContext eContext, StringBuilder sb)
            {
                Dump(eContext.Name, eContext.Description, sb, 3);

                Dump(eContext.Executables, sb);
            }

            private static void Dump(IEnumerable<ExContext> executableContexts, StringBuilder sb)
            {
                foreach (ExContext executableContext in executableContexts)
                {
                    Dump(executableContext.Name, executableContext.Description, sb, 6);

                    executableContext.Behaviors.ToList().ForEach(b => Dump(b.Name, b.Description, sb, 9));
                }
            }

            private static void Dump(string name, string description, StringBuilder sb, int indent)
            {
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "{0}[Name = {1}, Description = {2}]", string.Empty.PadLeft(indent), name, description));
            }
        }
    }
}