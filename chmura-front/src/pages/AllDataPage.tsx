import React, { useEffect, useState } from 'react'
import { PageWrapper } from '../components/PageWrapper'
import axios from 'axios'
import { baseRoot } from '../components/projectComponents'

export const AllDataPage = (props) => {
    const
        [data, setData] = useState(),
        getData = async () => {
            let req = await axios.get(baseRoot + "/Honey/GetAll")
            console.log(req)
            setData(req.data)
        }
    useEffect(() => {
        getData()
    }, [])
    return <PageWrapper>
        <h1>All data</h1>
    </PageWrapper>
}