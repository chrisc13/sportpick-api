using sportpick_domain;

public class AuthDTO{
    public AppUser? AppUser {get; set;}
    public string Message {get; set;}

    public AuthDTO(AppUser? appuser, string message){
        AppUser = appuser;
        Message = message;
    }
}
