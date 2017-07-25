using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MSIT11404project1.Models;
namespace MSIT11404project1.Areas.Accomodation.Models
{
    public class HouseQualityRepository:Repository<HouseQuality>,IRepository<HouseQuality>
    {
        //如果新增了一個檢舉負評 HouseMain的Badcomment要加一

        public void Addbadcomment(HouseQuality entity) {
            this.Create(entity);
            int hkeys = entity.HouseKey;
            HouseMainRepository house = new HouseMainRepository();
            HouseMain hs= house.GetByID(hkeys);
            int cm = Convert.ToInt32(hs.BadComment);
            hs.BadComment = cm + 1;
            house.Update(hs);

        }
        public class rank {
           public double ScoreA { get; set; }
           public int hkey { get; set; }
        } 


        //找有 評論分數 且 分數按照排名的 houses entity
        public List<HouseMain> housesScore() {
            var grouping =this.GetAll().Where(n=>n.Score!=-1).GroupBy(n => n.HouseKey).Select(n =>new {n.Key,n });
            List<rank> one = new List<rank>();
            
            HouseMainRepository houses = new HouseMainRepository();
            List<HouseMain> housese = houses.GetAll().AsEnumerable().ToList();
            foreach (var keys in grouping)
            {
                rank aa = new rank();
                var i = keys.n.Select(n => new  { ScoreA =  keys.n.Average(m => n.Score), hkey=keys.Key }).First();

                aa.ScoreA = Convert.ToDouble(i.ScoreA);
                aa.hkey = Convert.ToInt32(i.hkey);
                one.Add(aa);
            }

            var lists = from n in one
                        join ke in housese on n.hkey equals ke.HouseKey
                        orderby n.ScoreA descending
                        select ke;
            

            return lists.ToList();
        }

       //找到有負評 還沒被管理員確認的 
        public bool IsCommentNot(int hk) {

            var tt = this.GetAll().Where(n => n.HouseKey == hk&&n.Score==-1).ToList();
            if (tt.Count > 0)
            {
                tt = tt.Where(n => n.IsChecked == null).ToList();
                if (tt.Count > 0)
                {
                    return true;

                }
                else {
                    return false;
                }

                
            }
            else {
                return false;
            }
        }
    }
}