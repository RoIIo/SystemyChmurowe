import { useEffect, useState } from "react"
import { PageWrapper } from "../components/PageWrapper"
import axios from "axios"
import { baseRoot } from "../components/projectComponents"
import { AjaxWrapper } from "../components/AjaxWrapper"
import { defaultItem } from "./HoneyPage"

export const StatisticsPage = () => {
    const
        [statistics, setStatistics] = useState(null),
        [isPending, setPending] = useState(false),
        [filterName, setFilterName] = useState(""),
        [filterValue, setFilterValue] = useState(null),
        getStats = async () => {
            setPending(true)
            let req = await axios.get(baseRoot + "/Honey/GetStatistics")
            if (req.status == 200) {
                setStatistics(req.data)
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
            let req = await axios.get(baseRoot + "/Honey/GetStatistics" + filterQuery)

            if (req.status == 200) {
                setStatistics(req.data || null)
            }
        }
    useEffect(() => {
        getStats()
    }, [])
    return <PageWrapper>
        <h1>Statistics</h1>

        <AjaxWrapper isAjax={isPending}>
            <div className="filter">
                <div className="input-control">
                    <label htmlFor="filter">Property</label>
                    <select id='filter' name='filter' onChange={(e) => { setFilterName(e.target.value) }}>
                        {
                            Object.keys(defaultItem).map((k, idx) => <option value={k}>{k}</option>)
                        }
                    </select>
                </div>
                <div className="input-control">
                    <label htmlFor="filterValue">Value</label>
                    <input type="text" name='filterValue' id="filterValue" onChange={(e) => setFilterValue(e.target.value)} />
                </div>
                <button onClick={getDataFilters}>Filter</button>
            </div>
            {
                statistics && <div className="stats-table">
                    {
                        Object.keys(statistics).map(s => {
                            return <div className="stat-row">
                                <div className="label">{s}: </div>
                                <div className="value">{statistics[s]}</div>
                            </div>
                        })
                    }
                </div>
            }
        </AjaxWrapper>
    </PageWrapper>
}