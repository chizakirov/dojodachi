using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dojodachi.Models;
using Microsoft.AspNetCore.Http;

namespace dojodachi.Controllers
{
    public class HomeController : Controller
    { 
        [HttpGet("")]
        public IActionResult Index()
        {
            TempData["dachiStatus"] = "playing";
            int? energy = HttpContext.Session.GetInt32("energy");
            if (energy == null)
            {
                HttpContext.Session.SetInt32("happiness", 20);
                HttpContext.Session.SetInt32("fullness", 20);
                HttpContext.Session.SetInt32("energy", 50);
                HttpContext.Session.SetInt32("meals", 3);
                TempData["message"] = "Welcome! Your Dachi awaits to interact with you";
            }

            int? fullness = HttpContext.Session.GetInt32("fullness");
            int? happiness = HttpContext.Session.GetInt32("happiness");
            if(fullness <= 0 || happiness <= 0)
            {
                TempData["dachiStatus"] = "over";
                TempData["message"] = "Sadly, your Dachi just died. Hit Reset to start over";
            }
            else if(energy >= 100 && fullness >= 100 && happiness >= 100)
            {
                TempData["dachiStatus"] = "over";
                TempData["message"] = "Your Dachi can't be happier. You won!";
            }
            ViewBag.happiness = HttpContext.Session.GetInt32("happiness");
            ViewBag.fullness = HttpContext.Session.GetInt32("fullness");
            ViewBag.energy = HttpContext.Session.GetInt32("energy");
            ViewBag.meals = HttpContext.Session.GetInt32("meals");
            return View();
        }

        [HttpGet("feed")]
        public IActionResult Feed()
        {
            if (HttpContext.Session.GetInt32("meals") > 0)
            {
                int? meals = HttpContext.Session.GetInt32("meals") - 1;
                HttpContext.Session.SetInt32("meals", (int)meals);
                Random chance = new Random();
                int like = chance.Next(1, 5);
                if (like == 1)
                {
                    TempData["message"] = "Your dachi didn't like its meal";
                }
                else
                {
                    Random rand = new Random();
                    int y = rand.Next(5, 11);
                    int? fullness = HttpContext.Session.GetInt32("fullness") + y;
                    HttpContext.Session.SetInt32("fullness", (int)fullness);
                    TempData["message"] = $"You fed your dachi. Its fullness increased by {y} points, and meals decreased by 1";
                }
            }
            else
            {
                TempData["message"] = "You don't have any meals lefts to feed your dachi. Work to earn meals!";
            }

            return RedirectToAction("Index");
        }

        [HttpGet("play")]
        public IActionResult Play()
        {
            if (HttpContext.Session.GetInt32("energy") >= 5)
            {
                int? energy = HttpContext.Session.GetInt32("energy") - 5;
                HttpContext.Session.SetInt32("energy", (int)energy);
                Random chance = new Random();
                int like = chance.Next(1, 5);
                if (like == 1)
                {
                    TempData["message"] = "Your dachi didn't like playing";
                }
                else
                {
                    Random rand = new Random();
                    int x = rand.Next(5, 11);
                    int? happiness = HttpContext.Session.GetInt32("happiness");
                    HttpContext.Session.SetInt32("happiness", (int)happiness + x);
                    TempData["message"] = $"You played with your dachi and increased its happiness by {x} but its energy decreased by 5 points";
                }
            }
            else
            {
                TempData["message"] = "Your dachi doesn't have enough energy to play";
            }
            return RedirectToAction("Index");
        }

        [HttpGet("work")]
        public IActionResult Work()
        {
            int? energy = HttpContext.Session.GetInt32("energy");
            if(energy < 5)
            {
                TempData["message"] = "Your dachi doesn't have enough energy to work. It needs sleep to get refueled";
            }
            else{
                HttpContext.Session.SetInt32("energy", (int)energy -5);
                Random rand = new Random();
                int i = rand.Next(1,4);
                int? meals = HttpContext.Session.GetInt32("meals");
                HttpContext.Session.SetInt32("meals",(int)meals + i);
                TempData["message"] = $"Your dachi worked. It earned {i} meals and lost 5 energy points";
            }
            return RedirectToAction("Index");
        }

        [HttpGet("sleep")]
        public IActionResult Sleep()
        {
            int? energy = HttpContext.Session.GetInt32("energy") +15;
            HttpContext.Session.SetInt32("energy", (int)energy);

            int? fullness = HttpContext.Session.GetInt32("fullness") -5;
            HttpContext.Session.SetInt32("fullness", (int)fullness);

            int? happiness = HttpContext.Session.GetInt32("happiness") -5;
            HttpContext.Session.SetInt32("happiness", (int)happiness);

            TempData["message"] = "Your dachi slept. It gained 15 energy points, lost 5 fullness and 5 happiness points";
            return RedirectToAction("Index");
        }

        [HttpGet("reset")]
        public IActionResult Reset()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
