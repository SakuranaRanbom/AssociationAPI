using System.Linq;
using RfidAPI.Data;
using RfidAPI.Models;

namespace RfidAPI.Service
{
    public class TeamUserServiceImpl : TeamUserService
    {
        public DataContext _context;

        public TeamUserServiceImpl(DataContext context)
        {
            _context = context;
        }

        private bool changePer(PerLevel level, string userID, int teamID)
        {
            var relation = _context.TeamUsers.SingleOrDefault(s => s.TeamID == teamID && s.UserID.Equals(userID));
            if (relation == null)
            {
                return false;
            }
            else
            {
                if (relation.level == level)
                {
                    return false;
                }
                else
                {
                    relation.level = level;
                    
                }
            }

            if (_context.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool addUser(string userID, int teamID)
        {
            var relation = _context.TeamUsers.SingleOrDefault(s => s.UserID == userID && s.TeamID == teamID);
            if (relation == null)
            {
                _context.TeamUsers.Add(new TeamUser
                {
                    TeamUserID = null,
                    TeamID = teamID,
                    UserID = userID,
                    level = PerLevel.not
                });
            }

            if (_context.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            throw new System.NotImplementedException();
        }
        
        public bool toEditor(string userID, int teamID)
        {
            var relation = _context.TeamUsers.SingleOrDefault(s => s.TeamID == teamID && s.UserID.Equals(userID));
            if (relation == null)
            {
                return false;
            }
            else
            {
                if (relation.level == PerLevel.editor)
                {
                    return false;
                }
                else
                {
                    relation.level = PerLevel.editor;
                    
                }
            }

            if (_context.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            throw new System.NotImplementedException();
        }

        public bool toAdmin(string userID, int teamID)
        {
            if (changePer(PerLevel.admin, userID, teamID)) return true;
            else return false;
            throw new System.NotImplementedException();
            
        }

        public bool deleteUser(string userID, int teamID)
        {
            var relation = _context.TeamUsers.SingleOrDefault(s => s.TeamID == teamID && s.UserID.Equals(userID));
            if (relation == null)
            {
                return false;
            }
            else
            {
                _context.TeamUsers.Remove(relation);
            }

            if (_context.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            throw new System.NotImplementedException();
        }
    }
}