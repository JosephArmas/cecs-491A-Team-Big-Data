using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.Models
{
    public class AnalysisRow : IComparable<AnalysisRow>
    {
        public AnalysisRow()
        {
            month = 0;
            day = 0;
            featureCount = 0;
        }

        public AnalysisRow(int month, int day, int count)
        {
            this.month = month;
            this.day = day;
            this.featureCount = count;
        }

        public int month { get; set; }
        public int day { get; set; }
        public int featureCount { get; set; }

        public int CompareTo(AnalysisRow otherRow)
        {
            //returns which date came first
            //assumes to have a range of at most 3 months in a list
            if(this.month < 3 && otherRow.month > 10)
            {
                return 1;
            }
            else if(this.month > 10 && otherRow.month < 3)
            {
                return -1;
            }
            else if(this.month != otherRow.month)
            {
                return this.month - otherRow.month;
            }
            else
            {
                return this.day - otherRow.day;
            }
        }
    }
}
