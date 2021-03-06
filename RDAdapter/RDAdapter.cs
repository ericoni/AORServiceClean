﻿using FTN.Common;
using FTN.ServiceContracts;
using FTN.Services.NetworkModelService.DataModel.Core;
using FTN.Services.NetworkModelService.DataModel.Meas;
using FTN.Services.NetworkModelService.DataModel.Wires;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Adapter
{
    public class RDAdapter : IScadaGDAContract, INetworkModelClient
    {
        private NetworkModelGDAProxy gdaQueryProxy = null;
        private ModelResourcesDesc modelResourcesDesc = new ModelResourcesDesc();

        // Broj pokusaja uspostavljanja komunikacije
        private const int maxTry = 5;

        // Spavanje do narednog pokusaja
        private const int sleepTime = 3000;

        public RDAdapter()
        {
        }

        private NetworkModelGDAProxy GdaQueryProxy
        {
            get
            {
                if (gdaQueryProxy == null)
                {
                    int tryCounter = 0;

                    while (true)
                    {
                        if (tryCounter.Equals(maxTry))
                        {
                            throw new Exception("RAAdapter: Connection error.");
                        }

                        try
                        {
                            gdaQueryProxy = new NetworkModelGDAProxy("NetworkModelGDAEndpoint");
                            gdaQueryProxy.Open();

                            break;
                        }
                        catch (Exception)
                        {
                            tryCounter++;
                            Thread.Sleep(sleepTime);
                        }
                    }
                }

                return gdaQueryProxy;
            }
        }

        /// <summary>
        /// Metoda koja na osnovu gid-a vraca substation
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public Substation GetSubstation(long gid)
        {
            Substation substation = null;
            try
            {
                List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds(ModelCode.SUBSTATION);

                ResourceDescription rd = GdaQueryProxy.GetValues(gid, properties);
                substation = new Substation(rd.Id);
                substation.ConvertFromRD(rd);
                CommonTrace.WriteTrace(CommonTrace.TraceError, "Getting extent values method successfully finished.");

            }
            catch (Exception e)
            {
                string message = string.Format("Getting extent values method failed for {0}.\n\t{1}", ModelCode.ANALOGVALUE, e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }

            return substation;
        }

        public List<AnalogValue> GetAnalogValues()
        {
            int iteratorId = 0;
            List<long> ids = new List<long>();
            List<AnalogValue> analogValues = new List<AnalogValue>();

            try
            {
                int numberOfResources = 500;
                int resourcesLeft = 0;

                List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds(ModelCode.ANALOGVALUE);

                iteratorId = GdaQueryProxy.GetExtentValues(ModelCode.ANALOGVALUE, properties);
                resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);

                while (resourcesLeft > 0)
                {
                    List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

                    for (int i = 0; i < rds.Count; i++)
                    {
                        var rg = GdaQueryProxy.GetValues(rds[i].Id, properties);
                        AnalogValue analogValue = new AnalogValue(rds[i].Id);
                        analogValues.Add(analogValue.ConvertFromRD(rg));

                    }

                    resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
                }

                GdaQueryProxy.IteratorClose(iteratorId);

                CommonTrace.WriteTrace(CommonTrace.TraceError, "Getting extent values method successfully finished.");

            }
            catch (Exception e)
            {
                string message = string.Format("Getting extent values method failed for {0}.\n\t{1}", ModelCode.ANALOGVALUE, e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }

            return analogValues;
        }

        public List<DiscreteValue> GetDiscreteValues()
        {
            int iteratorId = 0;
            List<long> ids = new List<long>();
            List<DiscreteValue> discreteValues = new List<DiscreteValue>();

            try
            {
                int numberOfResources = 500;
                int resourcesLeft = 0;

                List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds(ModelCode.DISCRETEVALUE);

                iteratorId = GdaQueryProxy.GetExtentValues(ModelCode.DISCRETEVALUE, properties);
                resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);

                while (resourcesLeft > 0)
                {
                    List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

                    for (int i = 0; i < rds.Count; i++)
                    {
                        var rg = GdaQueryProxy.GetValues(rds[i].Id, properties);
                        DiscreteValue discreteValue = new DiscreteValue(rds[i].Id);
                        discreteValues.Add(discreteValue.ConvertFromRD(rg));

                    }

                    resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
                }

                GdaQueryProxy.IteratorClose(iteratorId);

                CommonTrace.WriteTrace(CommonTrace.TraceError, "Getting extent values method successfully finished.");

            }
            catch (Exception e)
            {
                string message = string.Format("Getting extent values method failed for {0}.\n\t{1}", ModelCode.DISCRETEVALUE, e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }

            return discreteValues;
        }

        public List<GeographicalRegion> GetRegions()
        {
            int iteratorId = 0;
            List<long> ids = new List<long>();
            List<GeographicalRegion> regions = new List<GeographicalRegion>();

            try
            {
                int numberOfResources = 500;
                int resourcesLeft = 0;

                List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds(ModelCode.REGION);

                iteratorId = GdaQueryProxy.GetExtentValues(ModelCode.REGION, properties);
                resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);

                while (resourcesLeft > 0)
                {
                    List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

                    for (int i = 0; i < rds.Count; i++)
                    {
                        var rg = GdaQueryProxy.GetValues(rds[i].Id, properties);
                        GeographicalRegion geographicalRegion = new GeographicalRegion(rds[i].Id);
                        regions.Add(geographicalRegion.ConvertFromRD(rg));

                    }

                    resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
                }

                GdaQueryProxy.IteratorClose(iteratorId);

                CommonTrace.WriteTrace(CommonTrace.TraceError, "Getting extent values method successfully finished.");

            }
            catch (Exception e)
            {
                string message = string.Format("Getting extent values method failed for {0}.\n\t{1}", ModelCode.REGION, e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }

            return regions;
        }

        public List<SubGeographicalRegion> GetSubRegionsForRegion(long regionGid)
        {
            List<SubGeographicalRegion> resultIds = new List<SubGeographicalRegion>();

            int numberOfResources = 500;
            Association association = new Association(ModelCode.REGION_SUBREGIONS, 0, false);

            try
            {
                List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds(ModelCode.SUBREGION);

                int iteratorId = GdaQueryProxy.GetRelatedValues(regionGid, properties, association);
                int resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);

                while (resourcesLeft > 0)
                {
                    List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

                    foreach (ResourceDescription rd in rds)
                    {
                        SubGeographicalRegion subRegion = new SubGeographicalRegion(rd.Id);
                        resultIds.Add(subRegion.ConvertFromRD(rd));
                    }

                    resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
                }

                GdaQueryProxy.IteratorClose(iteratorId);

                CommonTrace.WriteTrace(CommonTrace.TraceError, "Getting extent values method successfully finished.");
            }
            catch (Exception e)
            {
                string message = string.Format("Getting related values method  failed for sourceGlobalId = {0} and association (propertyId = {1}, type = {2}). Reason: {3}", regionGid, association.PropertyId, association.Type, e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }

            return resultIds;
        }

        public List<Substation> GetSubstationsForSubRegion(long subregionGid)
        {
            List<Substation> resultIds = new List<Substation>();

            int numberOfResources = 500;
            Association association = new Association(ModelCode.SUBREGION_SUBSTATIONS, 0, false);

            try
            {
                List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds(ModelCode.SUBSTATION);

                int iteratorId = GdaQueryProxy.GetRelatedValues(subregionGid, properties, association);
                int resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);

                while (resourcesLeft > 0)
                {
                    List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

                    foreach (ResourceDescription rd in rds)
                    {
                        Substation substation = new Substation(rd.Id);
                        resultIds.Add(substation.ConvertFromRD(rd));
                    }

                    resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
                }

                GdaQueryProxy.IteratorClose(iteratorId);

                CommonTrace.WriteTrace(CommonTrace.TraceError, "Getting extent values method successfully finished.");
            }
            catch (Exception e)
            {
                string message = string.Format("Getting related values method  failed for sourceGlobalId = {0} and association (propertyId = {1}, type = {2}). Reason: {3}", subregionGid, association.PropertyId, association.Type, e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }

            return resultIds;
        }

        public List<SynchronousMachine> GetDERs(long gid)
        {
            List<SynchronousMachine> retVal;
            List<Substation> substations = new List<Substation>();

            switch (((DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(gid)))
            {
                case DMSType.REGION:
                    List<SubGeographicalRegion> subRegions = GetSubRegionsForRegion(gid);
                    foreach (SubGeographicalRegion subRegion in subRegions)
                    {
                        substations.AddRange(GetSubstationsForSubRegion(subRegion.GlobalId));
                    }
                    break;
                case DMSType.SUBREGION:
                    substations = GetSubstationsForSubRegion(gid);
                    break;
                case DMSType.SUBSTATION:
                    substations.Add(new Substation(gid));
                    break;
                case DMSType.SYNCMACHINE:
                    SynchronousMachine sm = GetSyncMachineByGid(gid);
                    return new List<SynchronousMachine>() { sm };

            }

            retVal = GetSynchronousMachineForSubstations(substations);

            return retVal;
        }

        private List<SynchronousMachine> GetSynchronousMachineForSubstations(List<Substation> substations)
        {
            List<SynchronousMachine> resultIds = new List<SynchronousMachine>();

            int numberOfResources = 500;
            Association association = new Association(ModelCode.EQCONTAINER_EQUIPMENTS, 0, false);

            try
            {
                List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds(ModelCode.SYNCMACHINE);

                foreach (Substation substation in substations)
                {
                    int iteratorId = GdaQueryProxy.GetRelatedValues(substation.GlobalId, properties, association);
                    int resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);

                    while (resourcesLeft > 0)
                    {
                        List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

                        foreach (ResourceDescription rd in rds)
                        {
                            SynchronousMachine synchronousMachine = new SynchronousMachine(rd.Id);
                            resultIds.Add(synchronousMachine.ConvertFromRD(rd));
                        }

                        resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
                    }

                    GdaQueryProxy.IteratorClose(iteratorId);
                }
                CommonTrace.WriteTrace(CommonTrace.TraceError, "Getting extent values method successfully finished.");
            }
            catch (Exception e)
            {
                //string message = string.Format("Getting related values method  failed for sourceGlobalId = {0} and association (propertyId = {1}, type = {2}). Reason: {3}", regionGid, association.PropertyId, association.Type, e.Message);
                //Console.WriteLine(message);
                //CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }

            return resultIds;
        }

        public List<SynchronousMachine> GetAllDERs()
        {
            int iteratorId = 0;
            List<long> ids = new List<long>();
            List<SynchronousMachine> syncMachines = new List<SynchronousMachine>();

            try
            {
                int numberOfResources = 500;
                int resourcesLeft = 0;

                List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds(ModelCode.SYNCMACHINE);

                iteratorId = GdaQueryProxy.GetExtentValues(ModelCode.SYNCMACHINE, properties);
                resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);

                while (resourcesLeft > 0)
                {
                    List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

                    for (int i = 0; i < rds.Count; i++)
                    {
                        var rg = GdaQueryProxy.GetValues(rds[i].Id, properties);
                        SynchronousMachine syncMachine = new SynchronousMachine(rds[i].Id);
                        syncMachines.Add(syncMachine.ConvertFromRD(rg));
                    }

                    resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
                }

                GdaQueryProxy.IteratorClose(iteratorId);

                CommonTrace.WriteTrace(CommonTrace.TraceError, "Getting extent values method successfully finished.");

            }
            catch (Exception e)
            {
                string message = string.Format("Getting extent values method failed for {0}.\n\t{1}", ModelCode.SYNCMACHINE, e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }

            return syncMachines;
        }

        public List<long> GetAnalogValuesGidForGidAndPowerType(long gid, PowerType powerType)
        {
            List<long> resultIds = new List<long>();
            List<SynchronousMachine> ders = new List<SynchronousMachine>();

            int numberOfResources = 500;

            Association association = new Association(ModelCode.SYNCMACHINE_MEASVALUES, 0, false);

            if (((DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(gid)).Equals(DMSType.SYNCMACHINE))
            {
                ders.Add(new SynchronousMachine(gid));
            }
            else
            {
                ders = GetDERs(gid);
            }

            try
            {
                List<ModelCode> properties = new List<ModelCode>();
                properties.Add(ModelCode.IDOBJ_GID);
                properties.Add(ModelCode.MEASUREMENTVALUE_POWERTYPE);

                foreach (SynchronousMachine der in ders)
                {
                    int iteratorId = GdaQueryProxy.GetRelatedValues(der.GlobalId, properties, association);
                    int resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);

                    while (resourcesLeft > 0)
                    {
                        List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

                        foreach (ResourceDescription rd in rds)
                        {
                            PowerType type = (PowerType)rd.GetProperty(ModelCode.MEASUREMENTVALUE_POWERTYPE).AsEnum();
                            if (type.Equals(powerType))
                            {
                                resultIds.Add(rd.Id);
                            }
                        }

                        resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
                    }

                    GdaQueryProxy.IteratorClose(iteratorId);

                    CommonTrace.WriteTrace(CommonTrace.TraceError, "Getting extent values method successfully finished.");
                }
            }
            catch (Exception e)
            {
                string message = string.Format("Getting related values method  failed for sourceGlobalId = {0} and association (propertyId = {1}, type = {2}). Reason: {3}", gid, association.PropertyId, association.Type, e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }

            return resultIds;
        }

        public List<long> GetAllAnalogValuesGidForPowerType(PowerType powerType)
        {
            List<long> resultIds = new List<long>();
            List<AnalogValue> analogValues = new List<AnalogValue>();
            analogValues = GetAnalogValues();

            foreach (AnalogValue analogValue in analogValues)
            {
                if (analogValue.PowerType.Equals(powerType))
                {
                    resultIds.Add(analogValue.GlobalId);
                }
            }

            return resultIds;
        }

        public SynchronousMachine GetSyncMachineByGid(long gid)
        {
            SynchronousMachine syncMachine = null;

            try
            {
                List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds(ModelCode.SYNCMACHINE);

                ResourceDescription rd = GdaQueryProxy.GetValues(gid, properties);
                syncMachine = new SynchronousMachine(rd.Id);
                syncMachine.ConvertFromRD(rd);
                CommonTrace.WriteTrace(CommonTrace.TraceError, "Getting extent values method successfully finished.");

            }
            catch (Exception e)
            {
                string message = string.Format("Getting extent values method failed for {0}.\n\t{1}", ModelCode.SYNCMACHINE, e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }

            return syncMachine;
        }

        public GeographicalRegion GetRegionByGid(long gid)
        {
            GeographicalRegion syncMachine = null;

            try
            {
                List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds(ModelCode.REGION);

                ResourceDescription rd = GdaQueryProxy.GetValues(gid, properties);
                syncMachine = new GeographicalRegion(rd.Id);
                syncMachine.ConvertFromRD(rd);
                CommonTrace.WriteTrace(CommonTrace.TraceError, "Getting extent values method successfully finished.");

            }
            catch (Exception e)
            {
                string message = string.Format("Getting extent values method failed for {0}.\n\t{1}", ModelCode.REGION, e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }

            return syncMachine;
        }


        public SubGeographicalRegion GetSubRegionByGid(long gid)
        {
            SubGeographicalRegion syncMachine = null;

            try
            {
                List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds(ModelCode.SUBREGION);

                ResourceDescription rd = GdaQueryProxy.GetValues(gid, properties);
                syncMachine = new SubGeographicalRegion(rd.Id);
                syncMachine.ConvertFromRD(rd);
                CommonTrace.WriteTrace(CommonTrace.TraceError, "Getting extent values method successfully finished.");

            }
            catch (Exception e)
            {
                string message = string.Format("Getting extent values method failed for {0}.\n\t{1}", ModelCode.SUBREGION, e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }

            return syncMachine;
        }

        public List<Substation> GetAllSubstation()
        {
            int iteratorId = 0;
            List<long> ids = new List<long>();
            List<Substation> substations = new List<Substation>();

            try
            {
                int numberOfResources = 500;
                int resourcesLeft = 0;

                List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds(ModelCode.SUBSTATION);

                iteratorId = GdaQueryProxy.GetExtentValues(ModelCode.SUBSTATION, properties);
                resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);

                while (resourcesLeft > 0)
                {
                    List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

                    for (int i = 0; i < rds.Count; i++)
                    {
                        var rg = GdaQueryProxy.GetValues(rds[i].Id, properties);
                        Substation substation = new Substation(rds[i].Id);
                        substations.Add(substation.ConvertFromRD(rg));

                    }

                    resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
                }

                GdaQueryProxy.IteratorClose(iteratorId);

                CommonTrace.WriteTrace(CommonTrace.TraceError, "Getting extent values method successfully finished.");

            }
            catch (Exception e)
            {
                string message = string.Format("Getting extent values method failed for {0}.\n\t{1}", ModelCode.SUBSTATION, e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }

            return substations;
        }
    }
}
