using GameCore;
using VContainer;

namespace SaveLoadNamespace
{
    public sealed class ShiftArrowsSaveLoad : SaveLoad<ShiftArrowsData, ShiftArrowsDataConnector>
    {
        private readonly (int, int) _invalidArrowIndex = (-1, -1);
        private readonly bool _areActiveInit = true;

        protected override ShiftArrowsData ConvertDataToParams(ShiftArrowsDataConnector service)
        {
            service.SetupArrows();

            var data = new ShiftArrowsData(service.DisabledIndex, 
                service.AreActive, _invalidArrowIndex);

            return data;
        }

        protected override void LoadDefault(IObjectResolver context)
        {
            var service = context.Resolve<ShiftArrowsDataConnector>();

            var data = new ShiftArrowsData(_invalidArrowIndex,
                _areActiveInit, _invalidArrowIndex);

            service.SetArrowsData(data);
        }

        protected override void SetupParamsData(ShiftArrowsData paramsData, IObjectResolver context)
        {
            var service = context.Resolve<ShiftArrowsDataConnector>();

            service.SetArrowsData(paramsData);
        }
    }
}