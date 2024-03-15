using System.ComponentModel.DataAnnotations;

namespace CaravanApi.Models;

/// <summary> Contains all user info from the AAD. </summary>
public class User
{
    [Key]
    public int Id { get; set; }

    /// <example>Adriano</example>
    public string? FirstName { get; set; }

    /// <example> Chelentano </example>
    public string? LastName { get; set; }

    /// <example> adri123 </example>
    public string? Username { get; set; }
    
    /// <example>AdriCh_1234$</summary>
    public string? Password { get; set; }

    /// <summary> Is a long string with bunch of different chars.</summary>
    public string? Token { get; set; }

    /// <summary> Will be given to everyone. e.g. "User"</summary>
    /// <example>Admin</example>
    public string? Role { get; set; }

    /// <summary> Every user must provide email for ragistration.</summary>
    /// <example>müller.bong@gmail.com</example>
    public string? Email { get; set; }

    /// <summary> PasswordHash needed to hash password before saving it tohe DB. </summary>
    public byte[] PasswordHash { get; set; }

    /// <summary> PasswortSalt needed for hashing to be done. </summary>
    public byte[] PasswordSalt { get; set; }

    /// <summary> RefreshToken is accestoken which will be refreshed continuosly. </summary>
    public string? RefreshToken { get; set; }

    /// <summary> Expiry time of each token which was created. </summary>
    public DateTime RefreshTokenExpiryTime { get; set; }

    /// <summary> Token wixh user will need to reset password. </summary>
    public string? ResetPasswordToken { get; set; }

    /// <summary> Expiry time of ResetPassword token. </summary>
    public DateTime ResetPasswordExpiry { get; set; }
}
