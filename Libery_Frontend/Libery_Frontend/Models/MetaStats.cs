using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Libery_Frontend.Models
{
    public class MetaStats
    {
        private string _pageId, _pageName;
        private long _start;
        public MetaStats(string pageId, string pageName)
        {
            _pageId = pageId;
            _pageName = pageName;
            _start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        // Time on page is calculated and updated to database here
       public async Task Finish()
        {
  
            long end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long difference = end - _start;
            decimal duration = Math.Round((decimal)((float)difference / 1000.0f), 2);
            await UpdateDB(duration);

        }

        private async Task UpdateDB(decimal duration)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var db = new LibraryDBContext())
                    {
                        var row = db.MetaStatistics.SingleOrDefault(x => x.PageId == _pageId);

                        // In case no records where found
                        if (row == null)
                        {
                            MetaStatistic newEntity = new MetaStatistic()
                            {
                                PageId = _pageId,
                                PageName = _pageName,
                                Visits = 1,
                                Mean = (double)duration
                            };
                            db.MetaStatistics.Add(newEntity);
                            db.SaveChanges();
                            return;
                        }

                        // Update database with new mean (average) and new total visits
                        row.Mean = ((row.Visits * row.Mean) + (double)duration) / ++row.Visits;
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    // Do nothing
                    return;
                }
            }
            );
        }
    }
}
