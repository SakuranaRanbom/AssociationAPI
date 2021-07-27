using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RfidAPI.Models;

namespace RfidAPI.Service
{
    public interface TeamService
    {
        bool CreateTeam(string teamName, string teamInfo, string teamQQ, TeamStatus status);

        bool IsTeam(Team team);

        bool ConfirmTeam(string teamName);

        Task<ActionResult<IEnumerable<Team>>> getTeams();
        
        Task<ActionResult<IEnumerable<Team>>> getTeamByName(string name);
    }
}