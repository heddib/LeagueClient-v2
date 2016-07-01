using Kappa.Riot.Domain;
using System;
using System.Linq;

namespace Kappa.BackEnd.Server.Collection {
    [Docs("group", "Collection")]
    public class MasteriesService : JSONService {
        private MasteryBookDTO book;
        private Session session;

        public MasteriesService(Session session) : base("/collection/masteries") {
            this.session = session;

            session.Authed += Kappa_Authed;
        }

        private void Kappa_Authed(object sender, EventArgs e) {
            book = this.session.Me.Masteries;
        }

        [Endpoint("/get")]
        public MasteryBookDTO Get() {
            return book;
        }

        [Endpoint("/save")]
        public async void Save(MasteryBookPageDTO page) {
            var edited = book.BookPages.Single(p => p.PageId == page.PageId);
            book.BookPages.ForEach(p => p.Current = false);
            book.BookPages.Remove(edited);
            book.BookPages.Add(page);
            page.Current = true;

            await this.session.MasteryBookService.SaveMasteryBook(book);
        }

        [Endpoint("/select")]
        public async void Select(long pageId) {
            var selected = book.BookPages.Single(p => p.PageId == pageId);
            book.BookPages.ForEach(p => p.Current = false);
            selected.Current = true;

            await this.session.MasteryBookService.SaveMasteryBook(book);
        }
    }
}
