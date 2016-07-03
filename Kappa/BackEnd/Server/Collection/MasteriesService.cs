using Kappa.BackEnd.Server.Collection.Model;
using Kappa.Riot.Domain;
using System;
using System.Linq;

namespace Kappa.BackEnd.Server.Collection {
    [Docs("group", "Collection")]
    public class MasteriesService : JSONService {
        private MasteryBookDTO srcBook;
        private MasteryBook book;
        private Session session;

        public MasteriesService(Session session) : base("/collection/masteries") {
            this.session = session;

            session.Authed += Kappa_Authed;
        }

        private void Kappa_Authed(object sender, EventArgs e) {
            srcBook = this.session.Me.Masteries;

            book = new MasteryBook();
            foreach (var srcPage in srcBook.BookPages) {
                var page = new MasteryPage {
                    Id = srcPage.PageId,
                    Name = srcPage.Name
                };
                if (srcPage.Current) book.Selected = page.Id;

                foreach (var talent in srcPage.TalentEntries) {
                    page.Masteries.Add(talent.TalentId.ToString(), talent.Rank);
                }

                book.Pages.Add(page);
            }
        }

        [Endpoint("/get")]
        public MasteryBook Get() {
            return book;
        }

        [Endpoint("/save")]
        public async void Save(MasteryPage page) {
            var old = book.Pages.Single(p => p.Id == page.Id);
            book.Pages.Remove(old);
            book.Pages.Add(page);

            book.Selected = page.Id;

            var edited = srcBook.BookPages.Single(p => p.PageId == page.Id);
            srcBook.BookPages.ForEach(p => p.Current = false);
            edited.Current = true;
            edited.TalentEntries.Clear();

            foreach (var mastery in page.Masteries) {
                edited.TalentEntries.Add(new TalentEntry {
                    TalentId = int.Parse(mastery.Key),
                    Rank = mastery.Value
                });
            }

            await this.session.MasteryBookService.SaveMasteryBook(srcBook);
        }

        [Endpoint("/select")]
        public async void Select(long pageId) {
            book.Selected = pageId;

            var selected = srcBook.BookPages.Single(p => p.PageId == pageId);
            srcBook.BookPages.ForEach(p => p.Current = false);
            selected.Current = true;

            await this.session.MasteryBookService.SaveMasteryBook(srcBook);
        }
    }
}
