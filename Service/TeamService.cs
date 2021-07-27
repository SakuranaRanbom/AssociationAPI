using RfidAPI.Models;

namespace RfidAPI.Service
{
    public interface TeamService
    {
        bool CreateTeam(string teamName, string teamInfo, string teamQQ, TeamStatus status);

        bool IsTeam(Team team);

        bool ConfirmTeam(string teamName);
    }
}