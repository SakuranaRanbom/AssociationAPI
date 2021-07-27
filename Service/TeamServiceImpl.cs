using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using RfidAPI.Data;
using RfidAPI.Models;

namespace RfidAPI.Service
{
    public class TeamServiceImpl : TeamService
    {
        public DataContext _dataContext;

        public TeamServiceImpl(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public bool CreateTeam(string teamName, string teamInfo, string teamQQ, TeamStatus status)
        {
            var team = _dataContext.Teams.SingleOrDefault(s => s.teamName.Equals(teamName));
            if (team == null)
            {
                _dataContext.Teams.Add(new Team
                {
                    teamID = null,
                    teamName = teamName,
                    teamInfo = teamInfo,
                    teamQQ = teamQQ,
                    status = TeamStatus.not
                });
            }
            
            if (_dataContext.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            throw new System.NotImplementedException();
        }

        public bool IsTeam(Team team)
        {
            throw new System.NotImplementedException();
        }

        public bool ConfirmTeam(string teamName)
        {
            var team = _dataContext.Teams.SingleOrDefault(s => s.teamName.Equals(teamName));
            if (team == null)
            {
                return false;
            }
            else
            {
                team.status = TeamStatus.ok;
            }

            if (_dataContext.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            throw new System.NotImplementedException();
        }

        public async Task<ActionResult<IEnumerable<Team>>> getTeams()
        {
            return await _dataContext.Teams.ToListAsync();
        }

        public async Task<ActionResult<IEnumerable<Team>>> getTeamByName(string name)
        {
            return await _dataContext.Teams.Where(b => b.teamName.Contains(name)).ToListAsync();
        }
    }
}