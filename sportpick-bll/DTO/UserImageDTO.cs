public class UserProfileImageDTO{
    public string Username {get; set;}
    public string ProfileImageUrl {get; set;}

    public UserProfileImageDTO(string username, string profileImageUrl){
        Username = username;
        ProfileImageUrl = profileImageUrl;
    }
}
