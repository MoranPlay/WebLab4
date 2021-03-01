using System.Linq;
using System.Web.Mvc;
using WebLab4.Models;
using System.Data.Entity;
using System.Threading.Tasks;

namespace WebLab4.Controllers
{
    public class HomeController : Controller
    {
            SoccerContext db = new SoccerContext();

            // Выводим всех футболистов
            public ActionResult Index()
            {
                var players = db.Players.Include(p => p.Team);
                return View(players.ToList());
            }

        [HttpGet]
        public ActionResult Create()
        {
            // Формируем список команд для передачи в представление
            SelectList teams = new SelectList(db.Teams, "Id", "Name");
            ViewBag.Teams = teams;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Player player)
        {
            //Добавляем игрока в таблицу
            db.Players.Add(player);
            db.SaveChanges();
            // перенаправляем на главную страницу
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            // Находим в бд футболиста
            Player player = db.Players.Find(id);
            if (player != null)
            {
                // Создаем список команд для передачи в представление
                SelectList teams = new SelectList(db.Teams, "Id", "Name", player.TeamId);
                ViewBag.Teams = teams;
                return View(player);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(Player player)
        {
            db.Entry(player).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }





        /*     [HttpGet]
             public ActionResult Delete(int id)
             {
                 if (id == null)
                 {
                     return HttpNotFound();
                 }
                 // Находим в бд футболиста
                 Player player = db.Players.Find(id);
                 if (player != null)
                 {

                     return View(player);
                 }
                 return RedirectToAction("Index");
             }*/
        /*[HttpPost]
        public ActionResult Delete(Player player)
        {
            var players = db.Players.Where(p => p.TeamId == player.Id).Select(p => p);
            foreach (Player p in players)
            {
                p.TeamId = null;
                db.Entry(p).State = EntityState.Modified;
            }
            db.Players.Remove(player);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
*/
        /*[HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                Player user = db.Players.FirstOrDefault(p => p.Id == id);
                if (user != null)
                    return View(user);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public  ActionResult Delete(Player player)
        {
            if (player.Id != null)
            {
                Player user =db.Players.FirstOrDefault(p => p.Id == player.Id);
                if (user != null)
                {
                    db.Players.Remove(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }*/
        [HttpGet]
        [ActionName("Delete")]
        public async Task<ActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Player user = await db.Players.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                    return View(user);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Player user = await db.Players.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                {
                    db.Players.Remove(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return HttpNotFound();
        }




        public ActionResult TeamDetails(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Team team = db.Teams.Include(t => t.Players).FirstOrDefault(t => t.Id == id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }
    }
}