using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Project.Domain
{
    public class Book
    {
        private int id { get; set; }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string registId { get; set; }
        public string RegistId
        {
            get { return registId; }
            set { registId = value; }
        }

        private string name { get; set; }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string vol { get; set; }
        public string Vol
        {
            get { return vol; }
            set { vol = value; }
        }

        private string volExp;
        public string VolExp
        {
            get { return volExp; }
            set { volExp = value; }
        }

        private string writer;
        public string Writer
        {
            get { return writer; }
            set { writer = value; }
        }

        private string publisher;
        public string Publisher
        {
            get { return publisher; }
            set { publisher = value; }
        }

        private int publishYear;
        public int PublishYear
        {
            get { return publishYear; }
            set { publishYear = value; }
        }

        private string callNumber;
        public string CallNumber
        {
            get { return callNumber; }
            set { callNumber = value; }
        }

        private DateTime registDate;
        public DateTime RegistDate
        {
            get { return registDate; }
            set { registDate = value; }
        }

        private string state;
        public string State
        {
            get { return state; }
            set { state = value; }
        }
    }
}
