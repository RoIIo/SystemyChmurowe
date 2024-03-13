import { NavLink } from 'react-router-dom'
import { projectComponents } from './projectComponents'

export const MainLayout = (props: { children, [x: string]: any }) => {
    const
        menu = projectComponents.map((p, idx) => <NavLink key={idx} to={p.path}  >{p.label}</NavLink>)
    return <div className="layout chmurowe">
        <header>
            {menu}
        </header>
        <div className="main-content">
            {props.children}
        </div>
    </div>
}