using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using TeamAPI.Data;
using TeamAPI.Models;

namespace TeamAPI.Service
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

        public bool updateTeam(string userName, string teamName, string teamInfo, string teamQQ)
        {
            var _team = _dataContext.Teams.SingleOrDefault(s => s.teamName == teamName);
            var teamUser = _dataContext.TeamUsers.SingleOrDefault(s => s.UserID == userName && s.TeamID == _team.teamID);
            if (teamUser.level < PerLevel.editor)
            {
                return false;
            }
            else
            {
                _team.teamInfo = teamInfo;
                _team.teamQQ = teamQQ;
                if (_dataContext.SaveChanges() > 0) return true;
            }

            return true;
        }
        public bool IsTeam(Team team)
        {
            throw new System.NotImplementedException();
        }

        public bool IsTeam(string teamName)
        {
            var team = _dataContext.Teams.SingleOrDefault(s => s.teamName == teamName);
            if (team != null) return true;
            else return false;
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

        public bool DeleteTeam(int teamID)
        {
            var team = _dataContext.Teams.SingleOrDefault(s => s.teamID.Equals(teamID));
            if (team != null)
            {
                _dataContext.Teams.Remove(team);
            }
            else
            {
                return false;
            }

            if (_dataContext.SaveChanges() > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<ActionResult<IEnumerable<Team>>> getTeams()
        {
            return await _dataContext.Teams.ToListAsync();
        }

        public async Task<ActionResult<IEnumerable<Team>>> getOKTeams()
        {
            return await _dataContext.Teams.Where(b => b.status == TeamStatus.ok).ToListAsync();
        }
        public async Task<ActionResult<IEnumerable<Team>>> getTeamByName(string name)
        {
            return await _dataContext.Teams.Where(b => b.teamName.Contains(name)).ToListAsync();
        }

        public int? GetTeamID(string teamName)
        {
            var res = _dataContext.Teams.FirstOrDefault(s => s.teamName.Equals(teamName));
            if (res != null)
            {
                return res.teamID;
            }
            else
            {
                return null;
            }
        }
    }
}