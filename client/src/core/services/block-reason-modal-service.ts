import { Injectable } from '@angular/core';
import { BlockReasonModal } from '../../shared/block-reason-modal/block-reason-modal';

@Injectable({
  providedIn: 'root',
})
export class BlockReasonModalService {
  private modalComponent?: BlockReasonModal;

  register(component: BlockReasonModal){
      this.modalComponent = component;
    }

confirm(memberName: string, reason: string): Promise<string | null> {
    if (!this.modalComponent) {
      throw new Error('BlockReasonModal not registered');
    }
    return this.modalComponent.open(memberName, reason);
  }
}
