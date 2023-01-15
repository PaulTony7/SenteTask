using System.Collections.Generic;
using System;
using System.Linq;

namespace SenteTask
{
    class Company 
    {
        List<User> users;
        List<Transfer> transfers;
        User owner;
        float cutPercentage = 0.5f;

        public Company(string structurePath, string transfersPath)
        {
            if(!Input.LoadCompany(structurePath, out users, out owner))
                return;
            if(!Input.LoadTransfers(transfersPath, out transfers, users))
                return;

            ProcessTransfers();

            PrintReport();
        }

        public void ProcessTransfers()
        {
            if(transfers == null)
                return;
            foreach(Transfer transfer in transfers)
            {
                if(transfer.origin == owner || transfer.origin.supervisor == owner)
                    owner.commission += transfer.amount;
                else
                    transfer.origin.supervisor.commission += ProcessTransfer(transfer.origin.supervisor.supervisor, transfer.amount);
            }
        }

        uint ProcessTransfer(User user, uint amount)
        {
            //owner 50% cut
            if(user == owner)
            {
                uint cut = (uint)(amount * cutPercentage);
                user.commission += cut;
                return amount - cut;
            }
            //next in line supervisor 50% cut
            else
            {
                uint processAmount = ProcessTransfer(user.supervisor, amount);
                uint cut = (uint)(processAmount * cutPercentage);
                user.commission += cut;
                return cut;
            }

        }

        public void PrintReport()
        {
            users.Sort((a,b) => {return (int)(a.id - b.id);});
            foreach(User user in users)
            {
                Console.WriteLine(user.id + " " + user.Level + " " + user.EmptySubordinateAmount + " " + user.commission);
            }
        }
    }
}