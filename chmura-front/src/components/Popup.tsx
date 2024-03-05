import { useEffect } from "react"
import { Popup } from "reactjs-popup"
export const Modal = (props: { children: any, className?: string }) => {
    const
        { children, className = "" } = props

    useEffect(() => {

    }, [])

    return <Popup children={children} position={"center center"} />
}