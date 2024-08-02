namespace Back.Models;

public class FirebaseUser
{
    public string localId { get; set; }
    public string email { get; set; }
    public string displayName { get; set; }
    public string idToken { get; set; }
    public bool registered { get; set; }
    public string refreshToken { get; set; }
    public string expiresIn { get; set; }
}