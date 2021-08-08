namespace TeamAPI.Service
{
    public interface TeamUserService
    {
        public bool addUser(string userID, int teamID);

        public bool toEditor(string userID, int teamID);

        public bool toAdmin(string userID, int teamID);

        public bool deleteUser(string userID, int teamID);

        public bool IsAdmin(string userID);

        public bool HasTeam(string userID);
        public int? GetTeamID(string userID);
    }
}