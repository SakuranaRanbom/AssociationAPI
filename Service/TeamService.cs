using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TeamAPI.Models;

namespace TeamAPI.Service
{
    public interface TeamService
    {
        bool CreateTeam(string teamName, string teamInfo, string teamQQ, TeamStatus status);

        bool updateTeam(string userName, string teamName, string steamInfo, string teamQQ);
        bool IsTeam(Team team);
        bool IsTeam(string teamName);
        bool ConfirmTeam(string teamName);
        
        bool DeleteTeam(int teamID);
        Task<ActionResult<IEnumerable<Team>>> getTeams();
        Task<ActionResult<IEnumerable<Team>>> getOKTeams();
        Task<ActionResult<IEnumerable<Team>>> getTeamByName(string name);
        
        Task<ActionResult<IEnumerable<Team>>> getTeamByInfo(string info);

        int? GetTeamID(string teamName);
        
        bool SwitchTeamStatus(string teamName);
    }
}