import { useEffect, useState } from 'react'
import { PageWrapper } from '../components/PageWrapper'
import axios from 'axios'
import { baseRoot } from '../components/projectComponents'
import { Honey, Table } from '../components/Table'
import { AjaxWrapper } from '../components/AjaxWrapper'
import { HoneyForm } from '../components/HoneyForm'


export const defaultItem = {
    cs: 0,
    density: 0,
    ec: 0,
    f: 0,
    g: 0,
    id: null,
    pH: 0,
    pollen_analysis: "",
    purity: 0,
    viscosity: 0,
    wc: 0
} as Honey

export const HoneyPage = () => {
    const
        [data, setData] = useState<Honey[]>([]),
        [, setTotal] = useState(0),
        [filterName, setFilterName] = useState(""),
        [filterValue, setFilterValue] = useState(null),
        [isPending, setPending] = useState(false),
        [isAdd, setAdd] = useState(false),
        getData = async () => {
            setPending(true)
            let reqTotal = await axios.get(baseRoot + "/Honey/GetTotalEntities")
            if (reqTotal.status == 200) {
                setTotal(reqTotal.data || [])
                let filterQuery = ""
                if (filterName != "" && filterValue != null) {
                    filterQuery = `?Max${filterName}=${filterValue}`;
                }
                let reqEntries = await axios.get(baseRoot + "/Honey/GetAll" + filterQuery)
                if (reqEntries.status == 200) {
                    setData(reqEntries.data || [])
                }
            }
            setPending(false)
        },
        getDataFilters = async () => {
            let filterQuery = ""
            if (filterName != "" && filterValue != null) {
                filterQuery = `?Max${filterName}=${filterValue}&Min${filterName}=${filterValue}`;
            } else {
                return
            }
            if (filterName == "pollen_analysis") {
                filterQuery = `?PollenAnalysis=${filterValue}`;
            }
            let reqTotal = await axios.get(baseRoot + "/Honey/GetTotalEntities" + filterQuery)
            if (reqTotal.status == 200) {
                setTotal(reqTotal.data || [])
                let reqEntries = await axios.get(baseRoot + "/Honey/GetAll" + filterQuery)
                if (reqEntries.status == 200) {
                    setData(reqEntries.data || [])
                }
            }
        }
    useEffect(() => {
        getData()
    }, [])

    return <PageWrapper>
        <h1>Honey data entries</h1>
        <AjaxWrapper isAjax={isPending}>
            <div className="add-item">
                <button onClick={() => setAdd(true)}>Add Item</button>
                {isAdd &&
                    <HoneyForm item={defaultItem} refreshF={() => { getData(); setAdd(false) }} />}
            </div>
            Filter data based on property
            <div className="filter">
                <div className="input-control">
                    <label htmlFor="filter">Property</label>
                    <select id='filter' name='filter' onChange={(e) => { setFilterName(e.target.value) }}>
                        {
                            Object.keys(defaultItem).map((k,) => <option value={k}>{k}</option>)
                        }
                    </select>
                </div>
                <div className="input-control">
                    <label htmlFor="filterValue">Value</label>
                    <input type="text" name='filterValue' id="filterValue" onChange={(e) => setFilterValue(e.target.value)} />
                </div>
                <button onClick={getDataFilters}>Filter</button>
            </div>
            {data && <Table refreshF={getData} data={data} />}
        </AjaxWrapper>
    </PageWrapper>
}