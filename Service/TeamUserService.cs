namespace RfidAPI.Service
{
    public interface TeamUserService
    {
        public bool addUser(string userID, int teamID);

        public bool toEditor(string userID, int teamID);

        public bool toAdmin(string userID, int teamID);

        public bool deleteUser(string userID, int teamID);
    }
}