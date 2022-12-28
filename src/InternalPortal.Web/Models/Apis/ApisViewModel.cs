namespace InternalPortal.Web.Models.Apis
{
    public class ApisViewModel
    {
        public int Total { get; set; }
        public int Skipped { get; set; }
        public int Taken { get; set; }
        public List<ApiViewModel> Apis { get; set; }
                

        public bool ShowPreviousLink
        { get { return Skipped > 0; } }
        public int PreviousPage
        { get { return (int)Math.Ceiling((double)Skipped / Taken); } }
        public string PreviousPageLink
        { get { return $"/apis?skip={Skipped - Taken}&take={Taken}"; } }

        public bool ShowNextLink
        { get { return Skipped + Taken < Total; } }
        public int NextPage
        { get { return PreviousPage + 2; } }
        public string NextPageLink
        { get { return $"/apis?skip={Skipped + Taken}&take={Taken}"; } }
                
        public int TotalPages
        { get { return (int)Math.Ceiling((double)Total / Taken); } }

        public ApisViewModel(int? total, int skipped, int taken, List<ApiViewModel>? apis = null)
        {
            Total = total.HasValue ? total.Value : 0;
            Skipped = skipped;
            Taken = taken < 1 ? 1 : taken;
            Apis = apis != null ? apis : new List<ApiViewModel>();
        }
    }
}
