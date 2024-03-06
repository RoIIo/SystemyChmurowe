import React, { useEffect, useState } from 'react'
import { PageWrapper } from '../components/PageWrapper'
import axios from 'axios'
import { baseRoot } from '../components/projectComponents'
import { Honey, Table } from '../components/Table'
import { AjaxWrapper } from '../components/AjaxWrapper'
import { HoneyForm } from '../components/HoneyForm'


const defaultItem = {
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

export const HoneyPage = (props) => {
    const
        [data, setData] = useState<Honey[]>([]),
        [totalEntries, setTotal] = useState(0),
        [isPending, setPending] = useState(false),
        [isAdd, setAdd] = useState(false),
        getData = async () => {
            setPending(true)
            let reqTotal = await axios.get(baseRoot + "/Honey/GetTotalEntities")
            if (reqTotal.status == 200) {
                setTotal(reqTotal.data || [])
                let reqEntries = await axios.get(baseRoot + "/Honey/GetAll")
                if (reqEntries.status == 200) {
                    setData(reqEntries.data || [])
                }
            }
            setPending(false)
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
                    <HoneyForm item={defaultItem} />}
            </div>
            {data && <Table refreshF={getData} data={data} />}
        </AjaxWrapper>
    </PageWrapper>
}