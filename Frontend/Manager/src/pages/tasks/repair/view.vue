<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/tasks">{{locale.task.plural}}</Crumb>
    <Crumb :to="`/tasks/repair/${props.tab??'list'}?page=0`">{{locale.repairTask.plural}}</Crumb>
    <Crumb last>#{{props.id}}</Crumb>
</Bread>
<Fetch :url="`/manager/tasks/repair/view/${id}`" :cacheTTL="4"
    class-error="width-container card pad-1 mar-b-1">
    <template v-slot:default="{ data }">
        <RepairOrderView v-if="data.order" :order="data.order" 
            clickable @click="()=> goToOrder(data.order)"
            class="width-container card pad-1 mar-b-1" />
        <RepairTaskView :task="data" :canEdit="true"
            @click-assign="()=> { data.assigning = true }" 
            class="width-container card pad-1 mar-b-1" />
        <Modal v-if="data.assigning">
        <Fetch :url="`/manager/workers/list?role=${data.role}&page=0`" :cacheTTL="40" class-error="">
            <template v-slot:default="{ data: workers }">
                <WorkerSelect :workers="workers.items" :selection="data.worker"
                    @click-cancel="()=> { data.assigning = false }"
                    @click-submit="(worker)=> { data.assigning = false; tryAssign(data, worker) }"/>
            </template>
        </Fetch>
        </Modal> 
    </template>  
</Fetch>

</template>

<script setup>
import { useRouter } from "vue-router" 
import { ref, inject } from "vue" 
import { Fetch } from "@common/comp/special"
import { Bread, Crumb, Modal } from "@common/comp/layout"
import RepairTaskView from "@/comp/views/RepairTaskView.vue"
import RepairOrderView from "@/comp/mini/RepairOrderView.vue"
import WorkerSelect from "@/comp/mini/WorkerSelect.vue"

const router = useRouter()

const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    tab: String,
    id: Number
})

function tryAssign (data, worker) {
    console.log(worker)
    if (! (worker?.id)) return
    axios.post({
        url: `/manager/tasks/repair/assign/${data.id}`,
        params: { worker: worker.id }
    })
    .then(({ status, data: response }) => {
        if (status == 200 && response.success) {
            data.worker = worker
            data.workerId = worker.id
        }
        else {

        }
    })
    .catch(error => {
        console.error(error)
    })
}

function goToOrder(order) {
    router.push(`/orders/repair/view/${order.id}`)
}

</script>
