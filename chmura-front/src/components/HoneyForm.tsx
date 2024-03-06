import axios from "axios"
import { Honey } from "./Table"
import { useForm } from 'react-hook-form'
import { baseRoot } from "./projectComponents"

export interface HoneyFormProps {
    isEdit?: boolean,
    item: Honey,
    [x: string]: any
}

export const HoneyForm = (props: HoneyFormProps) => {
    const
        { item, isEdit = false, refreshF } = props,
        { register, handleSubmit } = useForm({ defaultValues: item }),
        sendForm = async (data, e) => {
            console.log(data, e)
            let req;
            if (isEdit) {
                req = await axios.put(baseRoot + "/Honey/UpdateHoney", data)
            }
            else {
                req = await axios.post(baseRoot + "/Honey/AddHoney", data)
            }
            if (req.status == 200)
                refreshF()
        }

    return <form action="" onSubmit={handleSubmit(sendForm)}>
        <div className="honey-form" >
            <div className="input-control">
                <label htmlFor="cs">cs</label>
                <input {...register("cs")} type="number" step=".001" />
            </div>
            <div className="input-control">
                <label htmlFor="density">density</label>
                <input {...register("density")} type="number" step=".001" />
            </div>
            <div className="input-control">
                <label htmlFor="wc">wc</label>
                <input {...register("wc")} type="number" step=".001" />
            </div>
            <div className="input-control">
                <label htmlFor="pH">pH</label>
                <input {...register("pH")} type="number" step=".001" />
            </div>
            <div className="input-control">
                <label htmlFor="ec">ec</label>
                <input {...register("ec")} type="number" step=".001" />
            </div>
            <div className="input-control">
                <label htmlFor="f">f</label>
                <input {...register("f")} type="number" step=".001" />
            </div>
            <div className="input-control">
                <label htmlFor="g">g</label>
                <input {...register("g")} type="number" step=".001" />
            </div>
            <div className="input-control">
                <label htmlFor="pollen_analysis">pollen_analysis</label>
                <input {...register("pollen_analysis")} type="text" />
            </div>
            <div className="input-control">
                <label htmlFor="viscosity">viscosity</label>
                <input {...register("viscosity")} type="number" step=".001" />
            </div>
            <div className="input-control">
                <label htmlFor="purity">purity</label>
                <input {...register("purity")} type="number" step=".001" />
            </div>
            <button>Save</button>
        </div>
    </form>
}