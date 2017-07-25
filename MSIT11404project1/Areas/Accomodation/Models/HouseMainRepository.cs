using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MSIT11404project1.Models;
namespace MSIT11404project1.Areas.Accomodation.Models
{
    public class HouseMainRepository : Repository<HouseMain>, IRepository<HouseMain>
    {
        //用memberid 找housekey,記得登入的時候把memberid存在cookie 裡
        public bool FindhouseKey(int? memberid, out List<int> housekeys)
        {
            housekeys = this.GetAll().AsEnumerable().Where(n => n.MemberID == Convert.ToInt32(memberid)).Select(n => n.HouseKey).ToList();
            if (housekeys.Count == 0)
            {

                return false;
            }
            else
            {
                return true;
            }

        }

        public List<HouseMain> SearchResaults(string CountryOrState, int Peoplenum, DateTime fdate, DateTime edate)
        {
            List<HouseMain> bigans = new List<HouseMain>();

            BedDetailsRepository beds = new BedDetailsRepository();
            CalendarRepository cals = new CalendarRepository();
            BookingRepository books = new BookingRepository();
            List<BedDtails> bedslists = new List<BedDtails>();
            //--------------------------------------第一步
            List<BedDtails> beds123 = new List<BedDtails>();//裝沒有在calendar裡的rooms

            List<BedDtails> beds111 = new List<BedDtails>();//裝有在calendar裡的 這個其實用Findcalendarbyhousekey就可以得到 只是為了分類而設
                                                            //--------------------------------------第二步
            List<Calendar> tests = new List<Calendar>();// (2-1)裝在calender且開放日期符合的Calendar 等著檢查是否通過book這一關
                                                        //------------------------------------------------不在cal中
            List<BedDtails> forlast = new List<BedDtails>();//裝經過book檢查後 不存在訂單紀錄的bedsrooms 可直接視為可被搜尋出來的housekeys

            List<int> forlasts2 = new List<int>();//裝經過book檢查 且日期符合的 透過這個在找到housekey就可以發布了
                                                  //------------------------------------------------------------------------------------------第三步
            List<Calendar> test2 = new List<Calendar>();//裝2-1的結果
            List<int> test3 = new List<int>();

            List<Calendar> calslists = new List<Calendar>();
            List<Booking> bookslists = new List<Booking>();


            //---------------------------------------------------------------------------------------第一步的一半
            List<int> justforh = new List<int>();

            //先透過名字找到housekey,如果housekey不在calendar裡?就是房東沒有設定開放日期

            List<int> housekeys = this.GetAll().AsEnumerable().Where(n => n.Country.Contains(CountryOrState) && n.PeopleAllowed > Peoplenum || n.State.Contains(CountryOrState)&&n.IsActive==true&&n.IsFreeze==false).Select(n => n.HouseKey).ToList();
            


            if (housekeys.Count == 0)
            {
                return null;
            }


            else
            {

                beds.FindBedsbyHouseKey(housekeys, out bedslists);//這張表是有符合housekeys的bedDetails

                


                var bedlist2 = bedslists.GroupBy(n => new { n.HouseKey, n.BedInSpaceID }).Select(n => new { Keys = n.Key, Groups = n }); //把bedDetails分成以housekeyID和Bedinspace一組



                var listhousekeyandBed = cals.FindCalendarByhkey(housekeys).GroupBy(n => new { n.HouseKeyID, n.BedInSpaceID }).Select(n => new { Keys = n.Key, Groups = n });//這個group是 "如果" calendar"有紀錄的" housekey和rooms


                if (listhousekeyandBed.Count() != 0)
                {
                    foreach (var keys in bedlist2)
                    {
                        int count1 = 0;
                        foreach (var ik in listhousekeyandBed)
                        {
                            if (keys.Keys.HouseKey == ik.Keys.HouseKeyID && keys.Keys.BedInSpaceID == ik.Keys.BedInSpaceID)
                            {
                                count1 += 1;

                            }
                            if (count1 == 1)
                            {
                                break;
                            }

                        }
                        if (count1 == 0)
                        {
                            foreach (var kks in keys.Groups)
                            {
                                beds123.Add(kks);
                            }

                        }

                       
                    }
                }

                else//這些housedetails都不在cal中
                {
                    List<Booking> aaa = new List<Booking>();

                    books.FindbookbyHouseKey(housekeys, out aaa);

                    var bbb = aaa.GroupBy(n => new { n.BedInSpaceID, n.HouseKeyID }).Select(n => new { eys = n.Key, group = n });

                    if (bbb != null)
                    {
                        foreach (var ims in bbb)
                        {
                            int count = 0;
                            foreach (var imss in ims.group)
                            {
                                if (fdate >= imss.EndDate || edate <= imss.FromDate) { }
                                else
                                {
                                    count += 1;
                                };



                            }//先把在booking中的cal檢查一下
                            if (count == 0)
                            {
                                justforh.Add(Convert.ToInt32(ims.eys.HouseKeyID));
                            }
                        }
                        foreach (var items in bedlist2)
                        {
                            int count2 = 0;
                            foreach (var ii in bbb)
                            {
                                if (items.Keys.HouseKey == ii.eys.HouseKeyID && items.Keys.BedInSpaceID == ii.eys.BedInSpaceID)
                                {
                                    count2 += 1;
                                }
                                if (count2 == 1)
                                {
                                    break;
                                }

                            }
                            if (count2 == 0)
                            {
                                justforh.Add(Convert.ToInt32(items.Keys.HouseKey));

                            }

                         

                        }

                        List<HouseMain> whats = new List<HouseMain>();

                        var inti = justforh.GroupBy(n => n).Select(n => n.Key);
                        foreach (var i in inti)
                        {
                            whats.Add(this.GetByID(i));

                        }
                        return whats;
                    }
                    else
                    {
                        List<HouseMain> nono = new List<HouseMain>();
                        nono = this.GetAll().AsEnumerable().Where(n => n.Country.Contains(CountryOrState) && n.PeopleAllowed > Peoplenum).ToList();
                        return nono;
                    }
                }//如果這些housekey在cal都沒有紀錄,透過housekeys帶出來的beddetails 要進去booking 篩選


                //如果 沒有 在cal裡的beddetails lists  不是空的
                if (beds123.Count != 0)
                {
                    books.FindbookbyHouseKey(housekeys, out bookslists);
                    var bookinggroup = bookslists.GroupBy(n => new { n.HouseKeyID, n.BedInSpaceID }).Select(n => new { bookeys = n.Key, groups = n });
                    var beds123group = beds123.GroupBy(n => new { n.HouseKey, n.BedInSpaceID }).Select(n => new { beds123keys = n.Key, groups = n });

                    foreach (var itemss in beds123group)
                    {
                        int count = 0;
                        foreach (var kke in bookinggroup)
                        {
                            if (itemss.beds123keys.HouseKey == kke.bookeys.HouseKeyID && itemss.beds123keys.BedInSpaceID == kke.bookeys.BedInSpaceID)
                            {
                                count += 1;

                            }
                            if (count == 1)
                            {
                                int count2 = 0;
                                foreach (var isis in kke.groups)
                                {
                                    if (fdate >= isis.EndDate || edate <= isis.FromDate) { }
                                    else
                                    {
                                        count2 += 1;
                                    }
                                }
                                if (count2 == 0)
                                {
                                    forlasts2.Add(Convert.ToInt32(kke.bookeys.HouseKeyID));
                                }
                                break;
                            }
                        }
                        if (count == 0)
                        {
                            foreach (var itemsss in itemss.groups)
                            {
                                forlast.Add(itemsss);
                            }
                        }
                    }
                }


                //如果 在cal 裡的beddetails lists 不是空的 判斷這些rooms的開放時間是否合乎限制
                if (beds111.Count != 0)
                {
                    foreach (var items in listhousekeyandBed)
                    {
                        foreach (var items2 in items.Groups)
                        {
                            if (items2.FromDate < fdate && items2.EndDate > edate)
                            {
                                tests.Add(items2);
                            }

                        }
                    }

                }

                if (tests.Count != 0)//合乎日期限制的 cal 判斷有沒有在 book裡
                {
                    var callistgroup = tests.GroupBy(n => new { n.HouseKeyID, n.BedInSpaceID }).Select(n => new { calKeys = n.Key, groups = n });
                    var bookinggroup = bookslists.GroupBy(n => new { n.HouseKeyID, n.BedInSpaceID }).Select(n => new { bookeys = n.Key, groups = n });
                    foreach (var classs in callistgroup)
                    {
                        int count = 0;
                        foreach (var isisis in bookinggroup)
                        {
                            if (isisis.bookeys.HouseKeyID == classs.calKeys.HouseKeyID && isisis.bookeys.BedInSpaceID == classs.calKeys.BedInSpaceID)
                            {
                                count += 1;

                            }
                            if (count == 1)
                            {
                                int count2 = 0;
                                foreach (var iis in isisis.groups)
                                {
                                    if (iis.FromDate >= edate || iis.EndDate <= fdate)
                                    {

                                    }
                                    else
                                    {
                                        count2 += 1;
                                    }
                                }
                                if (count2 == 0)
                                {
                                    test3.Add(Convert.ToInt32(isisis.bookeys.HouseKeyID));
                                }
                                break;

                            }
                        }
                        if (count == 0)
                        {
                            foreach (var items in classs.groups)
                            {
                                test2.Add(items);
                            }
                        }


                    }

                }

                List<int> anser = new List<int>();

                var one123 = forlast.Select(n => n.HouseKey).ToList();
                if (one123.Count != 0)
                {
                    foreach (var i in one123)
                    {
                        anser.Add(Convert.ToInt32(i));
                    }
                }
                var groupforlast2 = forlasts2.GroupBy(n => n).Select(n => n.Key);
                if (groupforlast2.Count() != 0)
                {
                    foreach (var i in groupforlast2)
                    {
                        anser.Add(i);
                    }

                }
                var three123 = test2.Select(n => n.HouseKeyID).ToList();
                if (three123.Count != 0)
                {
                    foreach (var i in three123)
                    {
                        anser.Add(i);
                    }
                }

                var four123 = test3.GroupBy(n => n).Select(n => n.Key);
                if (four123.Count() != 0)
                {
                    foreach (var i in four123)
                    {
                        anser.Add(i);
                    }
                }

                var ans = anser.GroupBy(n => n).Select(n => n.Key);
                foreach (var kke in ans)
                {
                    bigans.Add(this.GetByID(kke));
                }
                return bigans;

            }




        }


        public List<HouseMain> SearchResaults2(string CountryOrState, int Peoplenum, DateTime fdate, DateTime edate)
        {
            List<HouseMain> bigans = new List<HouseMain>();
            
            BedDetailsRepository beds = new BedDetailsRepository();
            CalendarRepository cals = new CalendarRepository();
            BookingRepository books = new BookingRepository();
            List<BedDtails> bedslists = new List<BedDtails>();

            //先透過名字找到housekey,如果housekey不在calendar裡?就是房東沒有設定開放日期

            List<int> housekeys = new List<int>();
            var aa = this.GetAll().AsEnumerable().Where(n => n.Country.Contains(CountryOrState) || n.State.Contains(CountryOrState));
            aa = aa.Where(n => n.PeopleAllowed >= Peoplenum&&n.IsActive==true);
            foreach (var bb in aa) {
                housekeys.Add(bb.HouseKey);
            }

            if (housekeys.Count == 0)
            {
                return null;
            }


            else
            {
                List<BedDtails> bedsInCal = new List<BedDtails>();
                List<BedDtails> bedsNocal = new List<BedDtails>();

                beds.FindBedsbyHouseKey(housekeys, out bedslists);//這張表是有符合housekeys的bedDetails

                foreach (var bedds in bedslists) {
                    if (cals.IsbedinCal(bedds.BedInSpaceID))
                    {
                        bedsInCal.Add(bedds);
                    }
                    else {
                        bedsNocal.Add(bedds);
                    }
                }
                

                //這個是判斷有在calendar裡且通過驗證的
                if (bedsInCal.Count > 0) {
                    
                List<BedDtails> bkeySuccess = new List<BedDtails>();//有通過calendar的床
                foreach (var bedss in bedsInCal) {
                    if (cals.GetAll().Where(n => n.BedInSpaceID == bedss.BedInSpaceID).ToList().Count > 0)
                        {
                            var bb = cals.GetAll().Where(n => n.BedInSpaceID == bedss.BedInSpaceID);
                            if (bb.Where(n => n.FromDate < fdate && n.EndDate > edate).ToList().Count > 0) { 
                            bkeySuccess.Add(bedss);

                            }

                    }

                }
                List<BedDtails> susccesinbooks = new List<BedDtails>();
                    if (bkeySuccess.Count > 0)
                    {
                        foreach (var bbs in bkeySuccess)

                        {
                            if (books.GetAll().Where(n => n.BedInSpaceID == bbs.BedInSpaceID).ToList().Count > 0)
                            {
                                var rr = books.GetAll().Where(n => n.BedInSpaceID == bbs.BedInSpaceID).ToList();
                                int all = rr.Count;
                                if(rr.Where(n=>n.FromDate >= edate || n.EndDate <= fdate).ToList().Count ==all) {
                                susccesinbooks.Add(bbs);
                                }
                            }
                            else if (books.GetAll().Where(n => n.BedInSpaceID == bbs.BedInSpaceID).ToList().Count() == 0) {
                                susccesinbooks.Add(bbs);
                            }
                         }

                    }
                    if (susccesinbooks.Count > 0) { 
                    bedsInCal.Clear();
                    bedsInCal = susccesinbooks;
                    }
                }
                
                if (bedsNocal.Count > 0) {
                    List<BedDtails> successinBook = new List<BedDtails>();
                    foreach(var bbs in bedsNocal)
                    {
                        if (books.GetAll().Where(n => n.BedInSpaceID == bbs.BedInSpaceID).ToList().Count > 0)
                        {
                            var ra = books.GetAll().Where(n => n.BedInSpaceID == bbs.BedInSpaceID).ToList();
                            int all = ra.Count;
                            if (ra.Where(n => n.FromDate>=edate||n.EndDate<=fdate).ToList().Count==all){ 
                            successinBook.Add(bbs);
                            }
                        }
                        else if (books.GetAll().Where(n => n.BedInSpaceID == bbs.BedInSpaceID).ToList().Count == 0) {
                            successinBook.Add(bbs);
                        }

                    }
                    if (successinBook.Count > 0) {
                        bedsNocal.Clear();
                        bedsNocal = successinBook;
                    }

                }

                if (bedsNocal.Count > 0 || bedsInCal.Count > 0)
                {
                   var keys = bedsInCal.Concat(bedsNocal).GroupBy(n => n.HouseKey).Select(n => n.Key);
                    List<HouseMain> finalhouses = new List<HouseMain>();
                    foreach (int key in keys) {

                        finalhouses.Add(this.GetByID(key));

                    }
                    return finalhouses;

                }
                else {
                    return null;
                }

            }
        }



        public int SearchByMidAndIsActive(int memid)
        {
            HouseMain entity = this.GetAll().Where(n => n.MemberID == memid && n.IsActive == false).First();

            return entity.HouseKey;
        }
        
    }
    }
