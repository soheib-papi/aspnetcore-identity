﻿namespace aspnetcore_identity.Models.Settings;

public class EmailSettings
{
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string SenderEmail { get; set; }
    public string Password { get; set; }
    public bool UseSsl { get; set; }
}
