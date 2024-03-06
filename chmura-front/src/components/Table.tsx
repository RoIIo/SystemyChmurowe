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
        { data, refreshF = () => { } } = props,
        [activeRowId, setRowId] = useState(-1),
        [editItem, setEditItem] = useState<Honey>(null),
        handleRowClick = (item, id) => {
            setRowId(id)
            setEditItem(item)
        }



    if (data.length == 0) return <></>
    return <>
        <table cellSpacing={0} className="data-table">
            <thead>
                <tr>
                    {Object.keys(data[0]).map((d, idx) => <th key={"-1" + idx}>{d}</th>)}
                    <th></th>
                </tr>
            </thead>
            <tbody>
                {data.map((d, rowidx) => {
                    return <>
                        <tr key={"rrr" + rowidx} className={`row ${rowidx % 2 && "alt-row"}`}>
                            {Object.keys(d).map((key, idx) => <td key={"" + d["id"] + idx}>
                                {d[key]}
                            </td>)}
                            <td key={"ddddddd"}>
                                <button onClick={() => handleRowClick(d, rowidx)}>Edit</button>
                            </td>
                        </tr>
                        {
                            rowidx == activeRowId &&
                            <tr key={"xxxxxr"}>
                                <td colSpan={Object.keys(d).length + 1}>
                                    <HoneyForm {...props} item={d} isEdit={true} />
                                </td>
                            </tr>
                        }
                    </>
                })}
            </tbody>
        </table>

    </>
}