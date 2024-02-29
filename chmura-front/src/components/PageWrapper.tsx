import React from 'react'

export const PageWrapper = (props)=>{
    return <div className="page page-wrapper">
        {props?.children}
    </div>
}