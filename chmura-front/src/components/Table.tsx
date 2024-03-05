import { useState } from "react"
import { Modal } from "./Popup"
import { HoneyForm } from "./HoneyForm"
import Popup from "reactjs-popup"

export interface ITableProps {
    data: Honey[]
    [x: string]: any
}
export interface Honey {
    id?: number,
    cs?: number,
    density?: number,
    wc?: number,
    pH?: number,
    ec?: number,
    f?: number,
    g?: number,
    pollen_analysis?: string,
    viscosity?: number,
    purity?: number
}
export const Table = (props: ITableProps) => {
    const
        { data } = props,
        [isPopup, setPopup] = useState(false)
     


    if (data.length == 0) return <></>
    return <>
        <table cellSpacing={0} className="data-table">
            <thead>
                {Object.keys(data[0]).map((d, idx) => <th key={"-1" + idx}>{d}</th>)}
                <th></th>
            </thead>
            <tbody>
                {data.map((d, idx) => {
                    return <tr className={`row ${idx % 2 && "alt-row"}`}>
                        {Object.keys(d).map((key, idx) => <td key={"" + d["id"] + idx}>
                            {d[key]}
                        </td>)}
                        <td>
                            <Popup position={"center center"} trigger={<button >Edit</button>}>
                                <HoneyForm item={d} isEdit={true} />
                                XDDDDDDDD
                            </Popup>
                        </td>
                    </tr>
                })}
            </tbody>
        </table>
    </>
}