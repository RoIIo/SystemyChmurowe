import { Honey } from "./Table"

export interface HoneyFormProps {
    isEdit?: boolean,
    item: Honey
}

export const HoneyForm = (props: HoneyFormProps) => {
    const
        { item, isEdit = false } = props;
    return <div className="honey-form">
        {item.id}
    </div>
}