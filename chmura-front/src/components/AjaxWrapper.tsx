export const AjaxWrapper = (props: { children: any, isAjax: boolean }) => {
    const
        { children, isAjax } = props;
    return isAjax ?
        <div className="ajax-loader">
            Please wait.
        </div>
        :
        <>
            {children}
        </>
}