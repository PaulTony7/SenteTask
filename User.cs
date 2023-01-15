using System.Collections.Generic;
namespace SenteTask
{
    public class User
    {
        public uint id;
        public uint commission = 0;
        public User supervisor;
        public List<User> subordinates;
        public uint EmptySubordinateAmount
        {
            get
            {
                uint _amount = 0;
                if(subordinates.Count == 0)
                    return _amount;
                foreach(User subordinate in subordinates)
                {
                    if(subordinate.EmptySubordinateAmount == 0)
                        _amount += 1;
                    else
                        _amount += subordinate.EmptySubordinateAmount;
                }
                return _amount;              
            }
        }

        public uint Level
        {
            get
            {
                if(supervisor == null)
                    return 0;
                else
                    return 1+supervisor.Level;
            }
        }
        public User(uint id, User supervisor=null)
        {
            this.id = id;
            this.supervisor = supervisor;
        }
    }
}