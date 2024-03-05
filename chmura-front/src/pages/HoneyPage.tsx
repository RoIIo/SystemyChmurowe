import React, { useEffect, useState } from 'react'
import { PageWrapper } from '../components/PageWrapper'
import axios from 'axios'
import { baseRoot } from '../components/projectComponents'
import { Honey, Table } from '../components/Table'
import { AjaxWrapper } from '../components/AjaxWrapper'

export const HoneyPage = (props) => {
    const
        [data, setData] = useState<Honey[]>([]),
        [isPending, setPending] = useState(false),
        getData = async () => {
            setPending(true)
            let req = await axios.get(baseRoot + "/Honey/GetAll")
            setData(req.data || [])
            setPending(false)
        }
    useEffect(() => {
        getData()
    }, [])
    return <PageWrapper>
        <h1>Honey data entries</h1>
        <AjaxWrapper isAjax={isPending}>
            <Table data={data} />
        </AjaxWrapper>
    </PageWrapper>
}