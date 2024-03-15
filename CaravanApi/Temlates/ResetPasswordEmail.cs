namespace CaravanApi.Temlates
{
    public class ResetPasswordEmail
    {
        public static string ResetPassowrsEmailBody(string email, string emailToken)
        {
            return $@"<html>
    <head>
        <body style=""margin: 0;padding: 0;font-family: Arial, Helvetica, sans-serif;"">
            <div style=""height:auto; background: linear-gradient(to top, #c9c9ff 50%, #6e6ef6) no-repeat; width: 400px; padding: 30px;"">
                <div>
                    <div>
                        <h1>Reset your Password</h1>
                        <hr>
                        <p>You're recieving this e-mail becouse you requested a password reset for your Rich-Shop account.</p>

                        <p>Please tap the button below to create a new password.</p>
                        <a href=""http://localhost:4200/reset-password?email={email}&code={emailToken}"" target=""_blank"" style=""background: #0d6efd; padding: 10px; border: none; color: white; border-radius: 4px; display: block; margin: 0 auto; width: 50%; text-align: center; text-decoration: none;"">Reset Password</a>
                        
                        <p>Kind Regards,<br><br>
                            Rich-Shop
                        </p>
                    </div>
                </div>
            </div>
        </body>
    </head>
</html>";
        }
    }
}
