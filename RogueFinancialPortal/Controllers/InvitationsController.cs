using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using RogueFinancialPortal.Enums;
using RogueFinancialPortal.Extensions;
using RogueFinancialPortal.Models;

namespace RogueFinancialPortal.Controllers
{
    [Authorize]
    public class InvitationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Invitations
       
        // GET: Invitations/Create
        //[Authorize(Roles = "Head")]
        public ActionResult Create()
        {
            var hhId = User.Identity.GetHouseHoldId();
            if(hhId == 0)
            {
                return RedirectToAction("Login", "Account");
            }
            var invitation = new Invitation((int)hhId);
            return View(invitation);
        }

        // POST: Invitations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( Invitation invitation)
        {
            if (ModelState.IsValid)
            {
                invitation.Code = Guid.NewGuid();
                db.Invitations.Add(invitation);
                db.SaveChanges();
                await invitation.SendInvitation();

                return RedirectToAction("Index","Home");
            }
           

            return View(invitation);
        }

        // GET: Invitations/Edit/5
       
        // GET: Invitations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            return View(invitation);
        }

        // POST: Invitations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invitation invitation = db.Invitations.Find(id);
            db.Invitations.Remove(invitation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
